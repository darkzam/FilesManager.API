using FilesManager.Application.Common.Interfaces;
using FilesManager.Domain.Models;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Drive.v3;
using Google.Apis.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace FilesManager.API.Controllers
{
    [ApiController]
    [Route("api/[Controller]")]
    public class FilesController : ControllerBase
    {
        private readonly IFileMetadataService _fileMetadataService;
        public FilesController(IFileMetadataService fileMetadataService)
        {
            _fileMetadataService = fileMetadataService ?? throw new ArgumentNullException(nameof(fileMetadataService));
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<FileMetadata>>> GetAll()
        {
            var result = await _fileMetadataService.GetAll();

            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<FileMetadata>> Get(Guid id)
        {
            var result = await _fileMetadataService.Get(id);

            return Ok(result);
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

        //[Authorize]
        [HttpGet("test")]
        public ActionResult GoogleDriveApi()
        {
            string[] Scopes = { DriveService.Scope.DriveReadonly };

            GoogleCredential credential = null;

            using (var stream =
               new FileStream("apikey.json", FileMode.Open, FileAccess.Read))
            {
                credential = GoogleCredential.FromStream(stream).CreateScoped(Scopes);
            }

            var service = new DriveService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = "FilesManager"
            });

            // Define parameters of request.
            FilesResource.ListRequest listRequest = service.Files.List();
            listRequest.PageSize = 30;
            listRequest.Fields = "nextPageToken, files(id, name, mimeType)";
            listRequest.SupportsAllDrives = true;
            listRequest.IncludeItemsFromAllDrives = true;
            listRequest.Q = "mimeType='image/jpeg'";

            // DrivesResource.ListRequest drivesRequest = service.Drives.List();

            //  IList<Google.Apis.Drive.v3.Data.File> drives = listRequest.Execute().Files;

            IList<Google.Apis.Drive.v3.Data.File> files = listRequest.Execute().Files;

            FilesResource.GetRequest getRequest = service.Files.Get(files[5].Id);

            var file = new MemoryStream();

            getRequest.DownloadWithStatus(file);

            var fileName = files[5].Name;
            var mimeType = "image/jpeg";

            file.Position = 0;

            return new FileStreamResult(file, mimeType)
            {
                FileDownloadName = fileName
            };
        }
    }
}
