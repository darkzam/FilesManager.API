using FilesManager.Application.Common.Interfaces;
using FilesManager.Domain.Models;
using FilesManager.Infrastructure.Contexts;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace FilesManager.Infrastructure.Repositories
{
    public class CategoryRepository : BaseRepository<Category>, ICategoryRepository
    {
        public CategoryRepository(FilesManagerContext filesManagerContext) : base(filesManagerContext)
        { }

        public async Task<Category> Find(string categoryDescription)
        {
            var categories = await _dbContext.Set<Category>().Where(x => x.Description.ToLower() == categoryDescription.ToLower()).ToListAsync();

            return categories.FirstOrDefault();
        }
    }
}
