using FilesManager.Domain.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FilesManager.Application.Common.Interfaces
{
    public interface ITagRepository
    {
        Task<IEnumerable<Tag>> GetAll();
        Task<Tag> Find(Guid id);
        Task<IEnumerable<Tag>> FindCollection(IEnumerable<Guid> ids);
        Tag Create(Tag tag);
        void CreateCollection(IEnumerable<Tag> tags);
        void Update(Tag tag);
        void UpdateCollection(IEnumerable<Tag> tags);
        void Remove(Tag tag);
        void RemoveCollection(IEnumerable<Tag> tags);
    }
}
