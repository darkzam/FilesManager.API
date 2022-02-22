using FilesManager.Application.Models;
using FilesManager.Domain.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FilesManager.Application.Common.Interfaces
{
    public interface ITagService
    {
        Task<IEnumerable<Tag>> CreateCollection(IEnumerable<Tag> filesMetadata);
        Task RemoveCollection(IEnumerable<Tag> tags);
        Task RemoveAssignments(FileMetadata fileMetadata);
        Task<FileSetTagsModel> AssignTags(FileMetadata file, IEnumerable<Tag> tags);
        Task<IEnumerable<Tag>> SearchByValue(IEnumerable<string> tags);
        Task<IEnumerable<Tag>> SearchTagsByFile(FileMetadata file);
        Task<IEnumerable<FileSearchModel>> SearchFilesByTags(IEnumerable<Tag> tags, int? limit);
        Task<IEnumerable<Tag>> ParseTags(IEnumerable<string> tags);
    }
}
