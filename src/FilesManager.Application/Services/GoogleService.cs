using FilesManager.Application.Common.Interfaces;
using FilesManager.Domain.Models;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Drive.v3;
using Google.Apis.Drive.v3.Data;
using Google.Apis.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace FilesManager.Application.Services
{
    public class GoogleService : IGoogleService
    {
        public GoogleService()
        { }

        private GoogleCredential GetCredential()
        {
            string[] Scopes = { DriveService.Scope.DriveReadonly };

            GoogleCredential credential = null;

            using (var stream =
               new FileStream("apikey.json", FileMode.Open, FileAccess.Read))
            {
                credential = GoogleCredential.FromStream(stream).CreateScoped(Scopes);
            }

            return credential;
        }

        public async Task<FileModel> Download(string id)
        {
            GoogleCredential credential = GetCredential();

            var service = new DriveService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = "FilesManager"
            });

            FilesResource.GetRequest getRequest = service.Files.Get(id);

            var file = await getRequest.ExecuteAsync();

            var fileStream = new MemoryStream();
            await getRequest.DownloadAsync(fileStream);
            fileStream.Position = 0;

            return new FileModel()
            {
                Name = file.Name,
                MimeType = file.MimeType,
                Content = fileStream
            };
        }

        public Task Upload(Google.Apis.Drive.v3.Data.File file)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Google.Apis.Drive.v3.Data.File>> GetAllFiles()
        {
            GoogleCredential credential = GetCredential();

            var service = new DriveService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = "FilesManager"
            });

            // Define parameters of request.
            FilesResource.ListRequest listRequest = service.Files.List();
            listRequest.Spaces = "drive";
            listRequest.PageSize = 1000;
            listRequest.Fields = "nextPageToken, files(id, name, mimeType)";
            listRequest.SupportsAllDrives = true;
            listRequest.IncludeItemsFromAllDrives = true;
            listRequest.Q = "mimeType!='application/vnd.google-apps.folder'";

            FileList fileList = await listRequest.ExecuteAsync();

            return fileList.Files;
        }
    }
}
