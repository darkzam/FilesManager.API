using FilesManager.Domain.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FilesManager.Application.Common.Interfaces
{
    public interface ITagService
    {
        Task<IEnumerable<Tag>> CreateCollection(IEnumerable<Tag> filesMetadata);
        Task RemoveCollection(IEnumerable<Guid> ids);
        Task<IEnumerable<FileMetadataTag>> AssignTags(FileMetadata file, IEnumerable<Tag> tags);
        Task<IEnumerable<Tag>> SearchByValue(IEnumerable<string> tags);
        Task<IEnumerable<Tag>> SearchTagsByFile(FileMetadata file);
        Task<IEnumerable<FileMetadata>> SearchFilesByTags(IEnumerable<string> tags, int limit);
    }
}
