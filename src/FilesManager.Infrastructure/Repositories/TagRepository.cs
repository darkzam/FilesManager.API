using FilesManager.Application.Common.Interfaces;
using FilesManager.Domain.Models;
using FilesManager.Infrastructure.Contexts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace FilesManager.Infrastructure.Repositories
{
    public class TagRepository : ITagRepository
    {
        private readonly FilesManagerContext _filesManagerContext;
        public TagRepository(FilesManagerContext filesManagerContext)
        {
            _filesManagerContext = filesManagerContext ?? throw new ArgumentNullException(nameof(filesManagerContext));
        }

        public Tag Create(Tag tag)
        {
            var result = _filesManagerContext.Tags.Add(tag);

            return result.Entity;
        }

        public void CreateCollection(IEnumerable<Tag> tags)
        {
            _filesManagerContext.Tags.AddRange(tags);
        }

        public async Task<Tag> Find(Guid id)
        {
            return await _filesManagerContext.Tags.FindAsync(id);
        }

        public async Task<IEnumerable<Tag>> FindCollection(IEnumerable<Guid> ids)
        {
            return await _filesManagerContext.Tags.Where(tag => ids.Contains(tag.Id)).ToListAsync();
        }

        public async Task<IEnumerable<Tag>> GetAll()
        {
            return await _filesManagerContext.Tags.ToListAsync();
        }

        public void Remove(Tag tag)
        {
            _filesManagerContext.Tags.Remove(tag);
        }

        public void RemoveCollection(IEnumerable<Tag> tags)
        {
            _filesManagerContext.Tags.RemoveRange(tags);
        }

        public void Update(Tag tag)
        {
            _filesManagerContext.Tags.Update(tag);
        }

        public void UpdateCollection(IEnumerable<Tag> tags)
        {
            _filesManagerContext.Tags.UpdateRange(tags);
        }

        public async Task<IEnumerable<Tag>> SearchBy(Expression<Func<Tag, bool>> predicate)
        {
            return await _filesManagerContext.Tags.Where(predicate).ToListAsync();
        }
    }
}
