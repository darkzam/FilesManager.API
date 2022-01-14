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

        public async Task<FileMetadata> SearchByRemoteId(string remoteId)
        {
            var result = await _unitOfWork.FileMetadataRepository.SearchBy(x => x.RemoteId == remoteId);

            if (result.Any())
            {
                return result.First();
            }

            return null;
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

        public async Task<FileMetadata> GetRandom(Category category)
        {
            var files = await _unitOfWork.FileMetadataRepository.SearchBy(x => x.Category.Id == category.Id);

            if (!files.Any())
            {
                return null;
            }

            var random = new Random();
            var prioritizeUntagged = random.Next(1, 100);

            if (prioritizeUntagged > 60)
            {
                return GetFullRandom(files);
            }

            return await PrioritizeUntagged(files);
        }

        private FileMetadata GetFullRandom(IEnumerable<FileMetadata> files)
        {
            var random = new Random();
            var row = random.Next(0, files.Count() - 1);

            return files.ElementAt(row);
        }

        private async Task<FileMetadata> PrioritizeUntagged(IEnumerable<FileMetadata> files)
        {
            var filesTags = await _unitOfWork.FileMetadataTagRepository.GetAll();

            var join = files.GroupJoin(filesTags,
                                        x => x.Id,
                                        y => y.FileMetadata.Id,
                                        (x, y) => new
                                        {
                                            File = x,
                                            Amount = y.Count()
                                        })
                             .OrderBy(x => x.Amount);

            var minFrequencies = join.Where(x => x.Amount == join.First().Amount);

            var random = new Random();
            var row = random.Next(0, minFrequencies.Count() - 1);

            return minFrequencies.ElementAt(row).File;
        }

        public async Task<Category> FindCategory(string categoryDescription)
        {
            var category = await _unitOfWork.CategoryRepository.Find(categoryDescription);

            return category;
        }
    }
}
