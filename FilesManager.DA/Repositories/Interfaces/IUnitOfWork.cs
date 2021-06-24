using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FilesManager.DA.Repositories.Interfaces
{
    public interface IUnitOfWork
    {
        IFileMetadataRepository FileMetadataRepository { get; }
        Task CompleteAsync();
    }
}
