using FilesManager.Application.Common.Interfaces;
using FilesManager.Domain.Models;
using FilesManager.Infrastructure.Contexts;

namespace FilesManager.Infrastructure.Repositories
{
    public class FileMetadataRepository : BaseRepository<FileMetadata>, IFileMetadataRepository
    {
        public FileMetadataRepository(FilesManagerContext filesManagerContext) : base(filesManagerContext)
        { }
    }
}
