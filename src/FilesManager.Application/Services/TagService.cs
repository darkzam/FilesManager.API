﻿using FilesManager.Application.Common.Interfaces;
using FilesManager.Application.Helpers;
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

        public async Task<IEnumerable<FileSearchModel>> SearchFilesByTags(IEnumerable<Tag> tags,
                                                                          int? limit)
        {
            var assignments = await _unitOfWork.FileMetadataTagRepository.GetAll();

            var filteredAssignments = assignments.Where(x => tags.Contains(x.Tag));

            var grouped = filteredAssignments.GroupBy(x => x.FileMetadata)
                                     .Select(group => new FileFrequency()
                                     {
                                         File = group.Key,
                                         Frequency = group.Count()
                                     })
                                    .OrderByDescending(x => x.Frequency);

            IEnumerable<FileFrequency> selection = null;

            if (limit.HasValue)
            {
                selection = grouped.Take(limit.Value);
            }
            else
            {
                var maxFrequencies = grouped.Where(x => x.Frequency == grouped.FirstOrDefault()?.Frequency);

                selection = (maxFrequencies.Any()) ? new List<FileFrequency>() { maxFrequencies.GetRandom() } : new List<FileFrequency>();
            }

            var associatedTags = selection.GroupJoin(assignments,
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

        public async Task<ParseResult> ParseTags(IEnumerable<string> tags)
        {
            if (!tags.Any())
            {
                new ParseResult()
                {
                    Tags = new List<Tag>(),
                    ParseOperations = new List<ParseOperation>()
                };
            }

            var existingTags = await _unitOfWork.TagRepository.GetAll();

            var parseOperations = new List<ParseOperation>();

            var parsedTags = existingTags.Where(x => IsBidirectionalSubstring(x.Value, tags, ref parseOperations));

            return new ParseResult()
            {
                Tags = parsedTags,
                ParseOperations = parseOperations
            };
        }

        private bool IsBidirectionalSubstring(string tag, IEnumerable<string> searchTags, ref List<ParseOperation> parseMap)
        {
            foreach (var searchTag in searchTags)
            {
                if (searchTag.Contains(tag) || tag.Contains(searchTag))
                {
                    parseMap.Add(new ParseOperation() { Input = searchTag, Ouput = tag });
                    return true;
                }
            }

            return false;
        }
    }
}
