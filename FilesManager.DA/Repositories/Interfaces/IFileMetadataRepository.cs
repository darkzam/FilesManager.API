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
        Task<IEnumerable<FileMetadata>> FindCollection(Guid id);
        Task<FileMetadata> Create(FileMetadata fileMetadata);
        Task<IEnumerable<FileMetadata>> CreateCollection(IEnumerable<FileMetadata> filesMetadata);
        Task<FileMetadata> Update(FileMetadata fileMetadata);
        Task<IEnumerable<FileMetadata>> UpdateCollection(IEnumerable<FileMetadata> filesMetadata);
        Task<bool> Remove(Guid id);
        Task<bool> RemoveCollection(IEnumerable<Guid> ids);
    }
}
