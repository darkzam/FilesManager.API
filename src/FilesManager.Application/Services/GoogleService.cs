using FilesManager.Application.Common.Interfaces;
using FilesManager.Application.Models;
using FilesManager.Domain.Models;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Drive.v3;
using Google.Apis.Drive.v3.Data;
using Google.Apis.Services;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace FilesManager.Application.Services
{
    public class GoogleService : IGoogleService
    {
        private readonly GoogleDriveSettings _options;
        public GoogleService(IOptions<GoogleDriveSettings> options)
        {
            _options = options.Value;
        }

        private GoogleCredential GetCredential(string[] scopes)
        {
            GoogleCredential credential = null;

            using (var stream =
               new FileStream("apikey.json", FileMode.Open, FileAccess.Read))
            {
                credential = GoogleCredential.FromStream(stream).CreateScoped(scopes);
            }

            return credential;
        }

        public async Task<FileModel> Download(string id)
        {
            GoogleCredential credential = GetCredential(new string[] { DriveService.Scope.DriveReadonly });

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

        public async Task Delete(string id)
        {
            GoogleCredential credential = GetCredential(new string[] { DriveService.Scope.DriveFile });

            var service = new DriveService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = "FilesManager"
            });

            FilesResource.DeleteRequest deleteRequest = service.Files.Delete(id);

            var result = await deleteRequest.ExecuteAsync();
        }

        public async Task<FileMetadata> Upload(FileModel fileModel)
        {
            try
            {
                GoogleCredential credential = GetCredential(new string[] { DriveService.Scope.DriveFile });

                var service = new DriveService(new BaseClientService.Initializer()
                {
                    HttpClientInitializer = credential,
                    ApplicationName = "FilesManager"
                });

                Google.Apis.Drive.v3.Data.File fileBody = new Google.Apis.Drive.v3.Data.File();
                fileBody.Name = fileModel.Name;
                fileBody.MimeType = fileModel.MimeType;
                fileBody.Parents = new List<string>() { _options.SharedFolderId };

                FilesResource.CreateMediaUpload uploadRequest = service.Files.Create(fileBody,
                                                                                     fileModel.Content,
                                                                                     fileModel.MimeType);
                uploadRequest.SupportsAllDrives = true;

                var result = await uploadRequest.UploadAsync();

                if (result.Status != Google.Apis.Upload.UploadStatus.Completed)
                {
                    return null;
                }

                return new FileMetadata()
                {
                    FileName = uploadRequest.ResponseBody.Name,
                    MimeType = uploadRequest.ResponseBody.MimeType,
                    RemoteId = uploadRequest.ResponseBody.Id,
                };
            }
            catch
            {
                throw;
            }
        }

        public async Task<IEnumerable<Google.Apis.Drive.v3.Data.File>> GetAllFiles()
        {
            try
            {
                GoogleCredential credential = GetCredential(new string[] { DriveService.Scope.DriveReadonly });

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
            catch
            {
                throw;
            }
        }
    }
}
