using FilesManager.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace FilesManager.Application.Common.Interfaces
{
    public interface ITagRepository
    {
        Task<IEnumerable<Tag>> GetAll();
        Task<Tag> Find(Guid id);
        Task<IEnumerable<Tag>> FindCollection(IEnumerable<Guid> ids);
        Tag Create(Tag tag);
        Task<IEnumerable<Tag>> SearchBy(Expression<Func<Tag, bool>> predicate);
        void CreateCollection(IEnumerable<Tag> tags);
        void Update(Tag tag);
        void UpdateCollection(IEnumerable<Tag> tags);
        void Remove(Tag tag);
        void RemoveCollection(IEnumerable<Tag> tags);
    }
}
