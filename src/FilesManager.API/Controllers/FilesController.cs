using FilesManager.API.Helpers;
using FilesManager.API.Models;
using FilesManager.Application.Common.Interfaces;
using FilesManager.Domain.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FilesManager.API.Controllers
{
    [ApiController]
    [Route("api/[Controller]")]
    public class FilesController : ControllerBase
    {
        private readonly IFileMetadataService _fileMetadataService;
        private readonly IGoogleService _googleService;
        public FilesController(IFileMetadataService fileMetadataService,
                               IGoogleService googleService)
        {
            _fileMetadataService = fileMetadataService ?? throw new ArgumentNullException(nameof(fileMetadataService));
            _googleService = googleService ?? throw new ArgumentNullException(nameof(googleService));
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

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(Guid id)
        {
            await _fileMetadataService.Remove(id);

            return Ok();
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

            var result = await _fileMetadataService.GetRandom(foundCategory);

            if (result is null)
            {
                return NotFound("There's not pics in the system.");
            }

            var resDto = new FileMetadataDto() { WebContentUrl = GoogleConstants.GenerateDownloadUrl(result.RemoteId) };

            return Ok(resDto);
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
