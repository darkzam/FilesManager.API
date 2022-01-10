﻿using FilesManager.Application.Common.Interfaces;
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
            var assignations = await _unitOfWork.FileMetadataTagRepository.SearchBy(x => x.FileMetadata.Id == file.Id);

            var newAssignations = tags.Where(x => !assignations.Any(y => y.Tag.Id == x.Id))
                                      .Select(x => new FileMetadataTag() { FileMetadata = file, Tag = x }).ToList();

            _unitOfWork.FileMetadataTagRepository.CreateCollection(newAssignations);

            await _unitOfWork.CompleteAsync();

            var newEntries = await _unitOfWork.FileMetadataTagRepository.FindCollection(newAssignations.Select(x => x.Id));

            return newEntries;
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

        public async Task RemoveCollection(IEnumerable<Guid> ids)
        {
            var entities = await _unitOfWork.TagRepository.FindCollection(ids);

            _unitOfWork.TagRepository.RemoveCollection(entities);

            await _unitOfWork.CompleteAsync();
        }

        public async Task<IEnumerable<Tag>> SearchByValue(IEnumerable<string> tags)
        {
            return await _unitOfWork.TagRepository.SearchBy(x => tags.Contains(x.Value));
        }

        public async Task<IEnumerable<FileMetadata>> SearchFilesByTags(IEnumerable<Tag> tags)
        {
            var assignments = await _unitOfWork.FileMetadataTagRepository.SearchBy(x => tags.Contains(x.Tag));

            var grouped = assignments.GroupBy(x => x.FileMetadata)
                                     .Select(group => new
                                     {
                                         File = group.Key,
                                         Frequency = group.Count()
                                     })
                                    .OrderByDescending(x => x.Frequency)
                                    .Take(3);

            return grouped.Select(x => x.File);
        }
    }
}