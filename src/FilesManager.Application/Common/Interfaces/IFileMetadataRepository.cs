using FilesManager.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace FilesManager.Application.Common.Interfaces
{
    public interface IFileMetadataRepository : IBaseRepository<FileMetadata>
    {
    }
}
