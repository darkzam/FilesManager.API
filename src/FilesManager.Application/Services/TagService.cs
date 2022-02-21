using FilesManager.Application.Common.Interfaces;
using FilesManager.Application.Models;
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

        public async Task<FileSetTagsModel> AssignTags(FileMetadata file, IEnumerable<Tag> tags)
        {
            var assignations = await _unitOfWork.FileMetadataTagRepository.SearchBy(x => x.FileMetadata.Id == file.Id);

            var newAssignations = tags.Where(x => !assignations.Any(y => y.Tag.Id == x.Id))
                                      .Select(x => new FileMetadataTag() { FileMetadata = file, Tag = x }).ToList();

            _unitOfWork.FileMetadataTagRepository.CreateCollection(newAssignations);

            await _unitOfWork.CompleteAsync();

            return new FileSetTagsModel()
            {
                RemoteId = file.RemoteId,
                Tags = assignations.Union(newAssignations).Select(x => x.Tag.Value),
                NewTags = newAssignations.Select(x => x.Tag.Value)
            };
        }

        public async Task<IEnumerable<Tag>> CreateCollection(IEnumerable<Tag> tags)
        {
            _unitOfWork.TagRepository.CreateCollection(tags);

            await _unitOfWork.CompleteAsync();

            var newEntries = await _unitOfWork.TagRepository.FindCollection(tags.Select(x => x.Id));

            return newEntries;
        }

        public async Task<IEnumerable<Tag>> SearchTagsByFile(FileMetadata file)
        {
            var assosiations = await _unitOfWork.FileMetadataTagRepository.SearchBy(x => x.FileMetadata.Id == file.Id);

            return assosiations.Select(x => x.Tag);
        }

        public async Task RemoveAssignments(FileMetadata fileMetadata)
        {
            var assignments = await _unitOfWork.FileMetadataTagRepository.SearchBy(x => x.FileMetadata.Id == fileMetadata.Id);

            _unitOfWork.FileMetadataTagRepository.RemoveCollection(assignments);

            await _unitOfWork.CompleteAsync();
        }

        public async Task RemoveCollection(IEnumerable<Tag> tags)
        {
            var assignments = await _unitOfWork.FileMetadataTagRepository.GetAll();

            var join = assignments.Join(tags, x => x.Tag.Id, y => y.Id, (x, y) => x);

            _unitOfWork.FileMetadataTagRepository.RemoveCollection(join);

            var entities = await _unitOfWork.TagRepository.FindCollection(tags.Select(x => x.Id));

            _unitOfWork.TagRepository.RemoveCollection(entities);

            await _unitOfWork.CompleteAsync();
        }

        public async Task<IEnumerable<Tag>> SearchByValue(IEnumerable<string> tags)
        {
            return await _unitOfWork.TagRepository.SearchBy(x => tags.Contains(x.Value));
        }

        public async Task<IEnumerable<FileSearchModel>> SearchFilesByTags(IEnumerable<string> tags, int limit)
        {
            var assignments = await _unitOfWork.FileMetadataTagRepository.GetAll();

            var filteredAssignments = assignments.Where(x => IsTagBidirectionalSubstring(x.Tag.Value, tags));

            var grouped = filteredAssignments.GroupBy(x => x.FileMetadata)
                                     .Select(group => new
                                     {
                                         File = group.Key,
                                         Frequency = group.Count()
                                     })
                                    .OrderByDescending(x => x.Frequency)
                                    .Take(limit);

            var associatedTags = grouped.GroupJoin(assignments,
                                                   x => x.File,
                                                   y => y.FileMetadata,
                                                   (x, y) => new FileSearchModel()
                                                   {
                                                       RemoteId = x.File.RemoteId,
                                                       Tags = y.Select(x => x.Tag.Value),
                                                       Matches = x.Frequency
                                                   });

            return associatedTags;
        }

        private bool IsTagBidirectionalSubstring(string tag, IEnumerable<string> searchTags)
        {
            foreach (var searchTag in searchTags)
            {
                if (searchTag.Contains(tag) || tag.Contains(searchTag))
                {
                    return true;
                }
            }

            return false;
        }
    }
}
