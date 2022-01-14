using FilesManager.Domain.Models;
using System.Threading.Tasks;

namespace FilesManager.Application.Common.Interfaces
{
    public interface ICategoryRepository
    {
        Task<Category> Find(string category);
    }
}
