using FilesManager.Application.Common.Interfaces;
using FilesManager.Infrastructure.Contexts;
using System;
using System.Threading.Tasks;

namespace FilesManager.Infrastructure.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly FilesManagerContext _filesManagerContext;
        public UnitOfWork(FilesManagerContext filesManagerContext)
        {
            _filesManagerContext = filesManagerContext ?? throw new ArgumentException(nameof(filesManagerContext));
        }

        public IFileMetadataRepository FileMetadataRepository => new FileMetadataRepository(_filesManagerContext);
        public IFileMetadataTagRepository FileMetadataTagRepository => new FileMetadataTagRepository(_filesManagerContext);
        public ITagRepository TagRepository => new TagRepository(_filesManagerContext);

        public async Task<int> CompleteAsync()
        {
            return await _filesManagerContext.SaveChangesAsync();
        }
    }
}
