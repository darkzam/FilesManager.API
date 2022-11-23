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
    public class FileMetadataTagRepository : BaseRepository<FileMetadataTag>, IFileMetadataTagRepository
    {
        public FileMetadataTagRepository(FilesManagerContext filesManagerContext) : base(filesManagerContext)
        { }

        public override async Task<IEnumerable<FileMetadataTag>> GetAll()
        {
            return await _dbContext.Set<FileMetadataTag>().Include(x => x.FileMetadata)
                                                          .Include(x => x.Tag)
                                                          .ToListAsync();
        }

        public override async Task<IEnumerable<FileMetadataTag>> SearchBy(Expression<Func<FileMetadataTag, bool>> predicate)
        {
            return await _dbContext.Set<FileMetadataTag>().Where(predicate)
                                                          .Include(x => x.FileMetadata)
                                                          .Include(x => x.Tag)
                                                          .ToListAsync();
        }
    }
}
