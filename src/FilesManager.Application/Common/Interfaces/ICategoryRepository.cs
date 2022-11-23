using FilesManager.Domain.Models;
using System.Threading.Tasks;

namespace FilesManager.Application.Common.Interfaces
{
    public interface ICategoryRepository : IBaseRepository<Category>
    {
        Task<Category> Find(string category);
    }
}
