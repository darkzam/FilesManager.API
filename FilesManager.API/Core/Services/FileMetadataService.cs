using FilesManager.API.Core.Services.Interfaces;
using FilesManager.DA.Models;
using FilesManager.DA.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FilesManager.API.Core.Services
{
    public class FileMetadataService : IFileMetadataService
    {
        private readonly IUnitOfWork _unitOfWork;
        public FileMetadataService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        }

        public async Task<FileMetadata> Create(FileMetadata fileMetadata)
        {
            var result = await _unitOfWork.FileMetadataRepository.Create(fileMetadata);

            return result;
        }

        public async Task<IEnumerable<FileMetadata>> CreateCollection(IEnumerable<FileMetadata> filesMetadata)
        {
            var result = await _unitOfWork.FileMetadataRepository.CreateCollection(filesMetadata);

            await _unitOfWork.CompleteAsync();

            return result;
        }

        public async Task<FileMetadata> Get(Guid id)
        {
            var result = await _unitOfWork.FileMetadataRepository.Find(id);

            return result;
        }

        public async Task<IEnumerable<FileMetadata>> GetAll()
        {
            var result = await _unitOfWork.FileMetadataRepository.GetAll();

            return result;
        }

        public async Task<bool> Remove(Guid id)
        {
            var result = await _unitOfWork.FileMetadataRepository.Remove(id);

            return result;
        }

        public async Task<bool> RemoveCollection(IEnumerable<Guid> ids)
        {
            var result = await _unitOfWork.FileMetadataRepository.RemoveCollection(ids);

            return result;
        }

        public async Task<FileMetadata> Update(FileMetadata fileMetadata)
        {
            var result = await _unitOfWork.FileMetadataRepository.Update(fileMetadata);

            return result;
        }

        public async Task<IEnumerable<FileMetadata>> UpdateCollection(IEnumerable<FileMetadata> filesMetadata)
        {
            var result = await _unitOfWork.FileMetadataRepository.UpdateCollection(filesMetadata);

            return result;
        }
    }
}
