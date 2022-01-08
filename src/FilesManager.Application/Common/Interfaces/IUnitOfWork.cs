using System.Threading.Tasks;

namespace FilesManager.Application.Common.Interfaces
{
    public interface IUnitOfWork
    {
        IFileMetadataRepository FileMetadataRepository { get; }
        IFileMetadataTagRepository FileMetadataTagRepository { get; }
        ITagRepository TagRepository { get; }
        Task<int> CompleteAsync();
    }
}
