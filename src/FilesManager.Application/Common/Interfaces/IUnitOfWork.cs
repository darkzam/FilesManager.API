using System.Threading.Tasks;

namespace FilesManager.Application.Common.Interfaces
{
    public interface IUnitOfWork
    {
        IFileMetadataRepository FileMetadataRepository { get; }
        Task<int> CompleteAsync();
    }
}
