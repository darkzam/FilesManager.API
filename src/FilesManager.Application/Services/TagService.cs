using FilesManager.Application.Common.Interfaces;
using FilesManager.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FilesManager.Application.Services
{
    public class TagService : ITagService
    {
        private readonly IUnitOfWork _unitOfWork;
        public TagService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        }

        public async Task<IEnumerable<FileMetadataTag>> AssignTags(FileMetadata file, IEnumerable<Tag> tags)
        {
            var assignations = tags.Select(x => new FileMetadataTag() { FileMetadata = file, Tag = x });

            _unitOfWork.FileMetadataTagRepository.CreateCollection(assignations);

            await _unitOfWork.CompleteAsync();

            var newEntries = await _unitOfWork.FileMetadataTagRepository.FindCollection(assignations.Select(x => x.Id));

            return newEntries;
        }

        public async Task<IEnumerable<Tag>> CreateCollection(IEnumerable<Tag> tags)
        {
            _unitOfWork.TagRepository.CreateCollection(tags);

            await _unitOfWork.CompleteAsync();

            var newEntries = await _unitOfWork.TagRepository.FindCollection(tags.Select(x => x.Id));

            return newEntries;
        }

        public async Task RemoveCollection(IEnumerable<Guid> ids)
        {
            var entities = await _unitOfWork.TagRepository.FindCollection(ids);

            _unitOfWork.TagRepository.RemoveCollection(entities);

            await _unitOfWork.CompleteAsync();
        }
    }
}
