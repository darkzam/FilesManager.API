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
            throw new NotImplementedException();
        }

        public Task<IEnumerable<FileMetadata>> CreateCollection(IEnumerable<FileMetadata> filesMetadata)
        {
            throw new NotImplementedException();
        }

        public Task<FileMetadata> Find(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<FileMetadata>> FindCollection(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<FileMetadata> Update(FileMetadata fileMetadata)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<FileMetadata>> UpdateCollection(IEnumerable<FileMetadata> filesMetadata)
        {
            throw new NotImplementedException();
        }

        public Task<bool> Remove(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<bool> RemoveCollection(IEnumerable<Guid> ids)
        {
            throw new NotImplementedException();
        }
    }
}
