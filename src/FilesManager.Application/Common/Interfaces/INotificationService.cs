using FilesManager.Domain.Models;
using System.Threading.Tasks;

namespace FilesManager.Application.Common.Interfaces
{
    public interface INotificationService
    {
        Task Notify(FileMetadata fileMetadata);
    }
}
