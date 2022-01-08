using FilesManager.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace FilesManager.Application.Common.Interfaces
{
    public interface IFileMetadataTagRepository
    {
        Task<IEnumerable<FileMetadataTag>> GetAll();
        Task<FileMetadataTag> Find(Guid id);
        Task<IEnumerable<FileMetadataTag>> FindCollection(IEnumerable<Guid> ids);
        Task<FileMetadataTag> SearchBy(Expression<Func<FileMetadata, bool>> predicate);
        FileMetadataTag Create(FileMetadataTag tag);
        void CreateCollection(IEnumerable<FileMetadataTag> fileMetadataTags);
        void Update(FileMetadataTag tag);
        void UpdateCollection(IEnumerable<FileMetadataTag> fileMetadataTags);
        void Remove(FileMetadataTag tag);
        void RemoveCollection(IEnumerable<FileMetadataTag> fileMetadataTags);
    }
}
