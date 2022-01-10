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
    public class FileMetadataTagRepository : IFileMetadataTagRepository
    {
        private readonly FilesManagerContext _filesManagerContext;
        public FileMetadataTagRepository(FilesManagerContext filesManagerContext)
        {
            _filesManagerContext = filesManagerContext ?? throw new ArgumentNullException(nameof(filesManagerContext));
        }

        public FileMetadataTag Create(FileMetadataTag tag)
        {
            var result = _filesManagerContext.FileMetadataTags.Add(tag);

            return result.Entity;
        }

        public void CreateCollection(IEnumerable<FileMetadataTag> fileMetadataTags)
        {
            _filesManagerContext.FileMetadataTags.AddRange(fileMetadataTags);
        }

        public async Task<FileMetadataTag> Find(Guid id)
        {
            return await _filesManagerContext.FileMetadataTags.FindAsync(id);
        }

        public async Task<IEnumerable<FileMetadataTag>> FindCollection(IEnumerable<Guid> ids)
        {
            return await _filesManagerContext.FileMetadataTags.Where(x => ids.Contains(x.Id)).ToListAsync();
        }

        public async Task<IEnumerable<FileMetadataTag>> GetAll()
        {
            return await _filesManagerContext.FileMetadataTags.Include(x => x.FileMetadata)
                                                              .Include(x => x.Tag)
                                                              .ToListAsync();
        }

        public void Remove(FileMetadataTag tag)
        {
            _filesManagerContext.FileMetadataTags.Remove(tag);
        }

        public void RemoveCollection(IEnumerable<FileMetadataTag> fileMetadataTags)
        {
            _filesManagerContext.FileMetadataTags.RemoveRange(fileMetadataTags);
        }

        public async Task<IEnumerable<FileMetadataTag>> SearchBy(Expression<Func<FileMetadataTag, bool>> predicate)
        {
            return await _filesManagerContext.FileMetadataTags.Where(predicate)
                                                              .Include(x => x.FileMetadata)
                                                              .Include(x => x.Tag)
                                                              .ToListAsync();
        }

        public void Update(FileMetadataTag tag)
        {
            _filesManagerContext.FileMetadataTags.Update(tag);
        }

        public void UpdateCollection(IEnumerable<FileMetadataTag> fileMetadataTags)
        {
            _filesManagerContext.FileMetadataTags.UpdateRange(fileMetadataTags);
        }
    }
}
