using FilesManager.DA.Contexts;
using FilesManager.DA.Models;
using FilesManager.DA.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FilesManager.DA
{
    public class FileMetadataRepository : IFileMetadataRepository
    {
        private readonly FilesManagerContext _filesManagerContext;
        public FileMetadataRepository(FilesManagerContext filesManagerContext)
        {
            _filesManagerContext = filesManagerContext ?? throw new ArgumentNullException(nameof(filesManagerContext));
        }

        public async Task<IEnumerable<FileMetadata>> GetAll()
        {
            return await _filesManagerContext.FileMetadata.ToListAsync();
        }

        public Task<FileMetadata> Create(FileMetadata fileMetadata)
        {
            var result = _filesManagerContext.FileMetadata.Add(fileMetadata);

            return Task.FromResult(result.Entity);
        }

        public Task CreateCollection(IEnumerable<FileMetadata> filesMetadata)
        {
            _filesManagerContext.FileMetadata.AddRange(filesMetadata);

            return Task.CompletedTask;
        }

        public async Task<FileMetadata> Find(Guid id)
        {
            return await _filesManagerContext.FileMetadata.FindAsync(id);
        }

        public async Task<IEnumerable<FileMetadata>> FindCollection(IEnumerable<Guid> ids)
        {
            return await _filesManagerContext.FileMetadata.Where(fileMetadata => ids.Contains(fileMetadata.Id)).ToListAsync();
        }

        public Task Update(FileMetadata fileMetadata)
        {
            _filesManagerContext.FileMetadata.Update(fileMetadata);

            return Task.CompletedTask;
        }

        public Task UpdateCollection(IEnumerable<FileMetadata> filesMetadata)
        {
            _filesManagerContext.FileMetadata.UpdateRange(filesMetadata);

            return Task.CompletedTask;
        }

        public Task Remove(FileMetadata fileMetadata)
        {
            _filesManagerContext.FileMetadata.Remove(fileMetadata);

            return Task.CompletedTask;
        }

        public Task RemoveCollection(IEnumerable<FileMetadata> filesMetadata)
        {
            _filesManagerContext.FileMetadata.RemoveRange(filesMetadata);

            return Task.CompletedTask;
        }
    }
}
