using FilesManager.Domain.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FilesManager.Application.Common.Interfaces
{
    public interface IFileMetadataService
    {
        Task<FileMetadata> Get(Guid Id);
        Task<IEnumerable<FileMetadata>> GetAll();
        Task<FileMetadata> SearchByRemoteId(string remoteId);
        Task<FileMetadata> SearchByFileName(string fileName);
        Task<FileMetadata> Create(FileMetadata fileMetadata);
        Task<IEnumerable<FileMetadata>> CreateCollection(IEnumerable<FileMetadata> filesMetadata);
        Task<FileMetadata> Update(FileMetadata fileMetadata);
        Task<IEnumerable<FileMetadata>> UpdateCollection(IEnumerable<FileMetadata> filesMetadata);
        Task Remove(Guid id);
        Task RemoveCollection(IEnumerable<Guid> ids);
        Task<FileMetadata> GetRandom(Category category);
        Task<Category> FindCategory(string category);
    }
}
