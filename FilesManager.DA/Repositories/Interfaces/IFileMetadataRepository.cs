using FilesManager.DA.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FilesManager.DA.Repositories.Interfaces
{
    public interface IFileMetadataRepository
    {
        Task<IEnumerable<FileMetadata>> GetAll();
        Task<FileMetadata> Find(Guid id);
        Task<IEnumerable<FileMetadata>> FindCollection(IEnumerable<Guid> ids);
        Task<FileMetadata> Create(FileMetadata fileMetadata);
        Task CreateCollection(IEnumerable<FileMetadata> filesMetadata);
        Task Update(FileMetadata fileMetadata);
        Task UpdateCollection(IEnumerable<FileMetadata> filesMetadata);
        Task Remove(FileMetadata fileMetadata);
        Task RemoveCollection(IEnumerable<FileMetadata> filesMetadata);
    }
}
