using FilesManager.Domain.Models;
using Google.Apis.Drive.v3.Data;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FilesManager.Application.Common.Interfaces
{
    public interface IGoogleService
    {
        Task<IEnumerable<File>> GetAllFiles();
        Task<FileModel> Download(string id);
        Task Upload(File file);
    }
}
