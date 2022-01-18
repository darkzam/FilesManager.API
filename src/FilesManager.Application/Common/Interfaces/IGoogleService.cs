using FilesManager.Domain.Models;
using Google.Apis.Drive.v3.Data;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FilesManager.Application.Common.Interfaces
{
    public interface IGoogleService
    {
        Task<IEnumerable<File>> GetAllFiles();
        Task Delete(string id);
        Task<FileModel> Download(string id);
        Task<FileMetadata> Upload(FileModel file);
    }
}
