using FilesManager.Domain.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FilesManager.Application.Common.Interfaces
{
    public interface IFileMetadataRepository
    {
        Task<IEnumerable<FileMetadata>> GetAll();
        Task<FileMetadata> Find(Guid id);
        Task<IEnumerable<FileMetadata>> FindCollection(IEnumerable<Guid> ids);
        FileMetadata Create(FileMetadata fileMetadata);
        void CreateCollection(IEnumerable<FileMetadata> filesMetadata);
        void Update(FileMetadata fileMetadata);
        void UpdateCollection(IEnumerable<FileMetadata> filesMetadata);
        void Remove(FileMetadata fileMetadata);
        void RemoveCollection(IEnumerable<FileMetadata> filesMetadata);
    }
}
