using FilesManager.Application.Common.Interfaces;
using FilesManager.Domain.Models;
using FilesManager.Infrastructure.Contexts;

namespace FilesManager.Infrastructure.Repositories
{
    public class TagRepository : BaseRepository<Tag>, ITagRepository
    {
        public TagRepository(FilesManagerContext filesManagerContext) : base(filesManagerContext)
        { }
    }
}
