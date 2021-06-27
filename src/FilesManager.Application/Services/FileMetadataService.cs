using FilesManager.Application.Common.Interfaces;
using FilesManager.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FilesManager.Application.Services
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

        public async Task Remove(Guid id)
        {
            var entity = await _unitOfWork.FileMetadataRepository.Find(id);

            _unitOfWork.FileMetadataRepository.Remove(entity);

            await _unitOfWork.CompleteAsync();
        }

        public async Task RemoveCollection(IEnumerable<Guid> ids)
        {
            var entities = await _unitOfWork.FileMetadataRepository.FindCollection(ids);

            _unitOfWork.FileMetadataRepository.RemoveCollection(entities);

            await _unitOfWork.CompleteAsync();
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
