using FilesManager.Application.Common.Interfaces;
using FilesManager.Domain.Models;
using FilesManager.Infrastructure.Contexts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace FilesManager.Infrastructure.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly FilesManagerContext _filesManagerContext;
        public CategoryRepository(FilesManagerContext filesManagerContext)
        {
            _filesManagerContext = filesManagerContext ?? throw new ArgumentNullException(nameof(filesManagerContext));
        }

        public async Task<Category> Find(string categoryDescription)
        {
            var categories = await _filesManagerContext.Category.Where(x => x.Description.ToLower() == categoryDescription.ToLower()).ToListAsync();

            return categories.FirstOrDefault();
        }
    }
}
