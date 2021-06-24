using FilesManager.DA.Contexts;
using FilesManager.DA.Repositories.Interfaces;
using System;
using System.Threading.Tasks;

namespace FilesManager.DA.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly FilesManagerContext _filesManagerContext;
        public UnitOfWork(FilesManagerContext filesManagerContext)
        {
            _filesManagerContext = filesManagerContext ?? throw new ArgumentException(nameof(filesManagerContext));
        }

        public IFileMetadataRepository FileMetadataRepository => new FileMetadataRepository(_filesManagerContext);

        public async Task CompleteAsync()
        {
            await _filesManagerContext.SaveChangesAsync();
        }
    }
}
