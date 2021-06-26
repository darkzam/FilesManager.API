using FilesManager.API.Core.Services.Interfaces;
using FilesManager.DA.Models;
using FilesManager.DA.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
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
            var result = _unitOfWork.FileMetadataRepository.Create(fileMetadata);

            await _unitOfWork.CompleteAsync();

            return result;
        }

        public async Task<IEnumerable<FileMetadata>> CreateCollection(IEnumerable<FileMetadata> filesMetadata)
        {
            _unitOfWork.FileMetadataRepository.CreateCollection(filesMetadata);

            await _unitOfWork.CompleteAsync();

            //loads the Inserted new entries with their respective Id
            var newEntries = await _unitOfWork.FileMetadataRepository.FindCollection(filesMetadata.Select(x => x.Id));

            return newEntries;
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
            var entity = await _unitOfWork.FileMetadataRepository.Find(id);

            _unitOfWork.FileMetadataRepository.Remove(entity);

            var count = await _unitOfWork.CompleteAsync();

            return count == 1;
        }

        public async Task<bool> RemoveCollection(IEnumerable<Guid> ids)
        {
            var entities = await _unitOfWork.FileMetadataRepository.FindCollection(ids);

            _unitOfWork.FileMetadataRepository.RemoveCollection(entities);

            var count = await _unitOfWork.CompleteAsync();

            return count == ids.Count();
        }

        public async Task<FileMetadata> Update(FileMetadata fileMetadata)
        {
            _unitOfWork.FileMetadataRepository.Update(fileMetadata);

            await _unitOfWork.CompleteAsync();

            var updatedEntry = await _unitOfWork.FileMetadataRepository.Find(fileMetadata.Id);

            return updatedEntry;
        }

        public async Task<IEnumerable<FileMetadata>> UpdateCollection(IEnumerable<FileMetadata> filesMetadata)
        {
            _unitOfWork.FileMetadataRepository.UpdateCollection(filesMetadata);

            await _unitOfWork.CompleteAsync();

            var updatedEntries = await _unitOfWork.FileMetadataRepository.FindCollection(filesMetadata.Select(x => x.Id));

            return updatedEntries;
        }
    }
}
