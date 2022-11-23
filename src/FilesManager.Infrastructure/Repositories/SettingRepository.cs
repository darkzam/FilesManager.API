using FilesManager.Application.Common.Interfaces;
using FilesManager.Domain.Models;
using FilesManager.Infrastructure.Contexts;

namespace FilesManager.Infrastructure.Repositories
{
    public class SettingRepository : BaseRepository<Setting>, ISettingRepository
    {
        public SettingRepository(FilesManagerContext filesManagerContext) : base(filesManagerContext)
        { }
    }
}
