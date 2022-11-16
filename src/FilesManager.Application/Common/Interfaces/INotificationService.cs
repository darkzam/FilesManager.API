using System.Threading.Tasks;

namespace FilesManager.Application.Common.Interfaces
{
    public interface INotificationService
    {
        Task Notify(string message);
    }
}
