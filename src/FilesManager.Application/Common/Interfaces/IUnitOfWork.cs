using System.Threading.Tasks;

namespace FilesManager.Application.Common.Interfaces
{
    public interface IUnitOfWork
    {
        IFileMetadataRepository FileMetadataRepository { get; }
        IFileMetadataTagRepository FileMetadataTagRepository { get; }
        ITagRepository TagRepository { get; }
        ICategoryRepository CategoryRepository { get; }
        ISettingRepository SettingRepository { get; }
        Task<int> CompleteAsync();
    }
}
