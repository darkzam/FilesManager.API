using FilesManager.API.Filters;
using FilesManager.API.Helpers;
using FilesManager.API.Models;
using FilesManager.Application.Common.Interfaces;
using FilesManager.Domain.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace FilesManager.API.Controllers
{
    [ServiceFilter(typeof(AuthorizationFilter))]
    [ApiController]
    [Route("api/[Controller]")]
    public class FilesController : ControllerBase
    {
        private readonly IFileMetadataService _fileMetadataService;
        private readonly IGoogleService _googleService;
        private readonly ITagService _tagService;
        private readonly INotificationService _notificationService;
        public FilesController(IFileMetadataService fileMetadataService,
                               IGoogleService googleService,
                               ITagService tagService,
                               INotificationService notificationService)
        {
            _fileMetadataService = fileMetadataService ?? throw new ArgumentNullException(nameof(fileMetadataService));
            _googleService = googleService ?? throw new ArgumentNullException(nameof(googleService));
            _tagService = tagService ?? throw new ArgumentNullException(nameof(tagService));
            _notificationService = notificationService ?? throw new ArgumentNullException(nameof(notificationService));
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<FileMetadataDto>>> GetAll()
        {
            var result = await _fileMetadataService.GetAll();

            var resultDto = result.Select(x => new FileMetadataDto() { WebContentUrl = GoogleConstants.GenerateDownloadUrl(x.RemoteId) });

            return Ok(resultDto);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<FileMetadataDto>> Get(Guid id)
        {
            var result = await _fileMetadataService.Get(id);

            if (result is null)
            {
                NotFound();
            }

            var resDto = new FileMetadataDto() { WebContentUrl = GoogleConstants.GenerateDownloadUrl(result.RemoteId) };

            return Ok(resDto);
        }

        [HttpGet("{id}/download")]
        public async Task<ActionResult<FileMetadata>> Download(Guid id)
        {
            var fileMetadata = await _fileMetadataService.Get(id);

            var fileModel = await _googleService.Download(fileMetadata.RemoteId);

            return new FileStreamResult(fileModel.Content, fileModel.MimeType)
            {
                FileDownloadName = fileModel.Name
            };
        }

        [HttpPost]
        public async Task<ActionResult<FileMetadata>> Create([FromBody] FileMetadata fileMetadata)
        {
            var result = await _fileMetadataService.Create(fileMetadata);

            return Ok(result);
        }

        [HttpPost("collection")]
        public async Task<ActionResult<IEnumerable<FileMetadata>>> CreateCollection([FromBody] IEnumerable<FileMetadata> filesMetadata)
        {
            var result = await _fileMetadataService.CreateCollection(filesMetadata);

            return Ok(result);
        }

        [HttpPut]
        public async Task<ActionResult> Update([FromBody] FileMetadata fileMetadata)
        {
            var result = await _fileMetadataService.Update(fileMetadata);

            return Ok(result);
        }

        [HttpPut("collection")]
        public async Task<ActionResult> UpdateCollection([FromBody] IEnumerable<FileMetadata> filesMetadata)
        {
            var result = await _fileMetadataService.UpdateCollection(filesMetadata);

            return Ok(result);
        }

        [HttpDelete("{remoteId}")]
        public async Task<ActionResult> Delete(string remoteId)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(remoteId))
                {
                    return BadRequest($"{nameof(remoteId)} is null");
                }

                var file = await _fileMetadataService.SearchByRemoteId(remoteId);

                if (file is null)
                {
                    return NotFound("File with provided remoteId was not in the system.");
                }

                var result = await _googleService.Delete(file.RemoteId);

                if (!result)
                {
                    //refactor this into a new exception from within the method ex. throw new GoogleException().
                    return StatusCode(StatusCodes.Status500InternalServerError, "Remote deletion failed.");
                }

                await _tagService.RemoveAssignments(file);

                await _fileMetadataService.Remove(file.Id);

                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpDelete("collection")]
        public async Task<ActionResult> DeleteCollection([FromBody] IEnumerable<Guid> ids)
        {
            await _fileMetadataService.RemoveCollection(ids);

            return Ok();
        }

        [HttpGet("random")]
        public async Task<ActionResult<FileMetadataDto>> GetRandomFile([FromQuery] string category)
        {
            var foundCategory = await _fileMetadataService.FindCategory(category);

            if (foundCategory is null)
            {
                return NotFound(nameof(category));
            }

            var file = await _fileMetadataService.GetRandom(foundCategory);

            if (file is null)
            {
                return NotFound("There's not pics in the system.");
            }

            var tags = await _tagService.SearchTagsByFile(file);

            var resDto = new FileMetadataDto()
            {
                WebContentUrl = GoogleConstants.GenerateDownloadUrl(file.RemoteId),
                Tags = tags.Select(x => x.Value)
            };

            return Ok(resDto);
        }

        [HttpPost("upload")]
        public async Task<ActionResult> UploadFile([FromForm] IFormFile formFile)
        {
            try
            {
                if (formFile is null)
                {
                    return BadRequest(nameof(formFile));
                }

                if (formFile.Length == 0)
                {
                    return BadRequest("FileContent is empty");
                }

                //check if valid fileType
                var category = await _fileMetadataService.FindCategory(formFile.ContentType.GetCategory());

                if (category is null)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, "File Category does not exist in the system.");
                }

                //search duplicate filename
                var existingFile = await _fileMetadataService.SearchByFileName(formFile.FileName);

                if (existingFile != null)
                {
                    return BadRequest($"The {nameof(formFile.FileName)} provided already exists in the system.");
                }

                var content = new MemoryStream();
                await formFile.CopyToAsync(content);

                var fileModel = new FileModel()
                {
                    Name = formFile.FileName,
                    MimeType = formFile.ContentType,
                    Content = content
                };

                //Upload File To GoogleDrive
                var fileMetadata = await _googleService.Upload(fileModel);

                if (fileMetadata is null)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, "GoogleDrive upload failed.");
                }

                fileMetadata.Category = category;

                //Create File Metadata
                var result = await _fileMetadataService.Create(fileMetadata);

                await _notificationService.Notify(fileMetadata);

                return Ok(result);
            }
            catch (Exception ex)
            {
                //TODO: Add Serilog.
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpGet("seedData")]
        public async Task<ActionResult<IEnumerable<FileMetadata>>> SeedDBFromRemote()
        {
            var filesFromRepo = await _googleService.GetAllFiles();

            var files = filesFromRepo.Select(x => new FileMetadata()
            {
                FileName = x.Name,
                MimeType = x.MimeType,
                RemoteId = x.Id
            });

            var result = await _fileMetadataService.CreateCollection(files);

            return Ok(result);
        }
    }
}
