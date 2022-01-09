using FilesManager.API.Models;
using FilesManager.Application.Common.Interfaces;
using FilesManager.Domain.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FilesManager.API.Controllers
{
    [ApiController]
    [Route("api/[Controller]")]
    public class TagsController : ControllerBase
    {
        private readonly ITagService _tagService;
        private readonly IFileMetadataService _fileMetadataService;
        public TagsController(IFileMetadataService fileMetadataService,
                              ITagService tagService)
        {
            _fileMetadataService = fileMetadataService ?? throw new ArgumentNullException(nameof(fileMetadataService));
            _tagService = tagService ?? throw new ArgumentNullException(nameof(tagService));
        }

        [HttpPost]
        public async Task<ActionResult<IEnumerable<FileMetadataTag>>> Create(FileMetadataTagsDto fileTagsDto)
        {
            var file = await _fileMetadataService.SearchByRemoteId(fileTagsDto.RemoteId);

            if (file is null)
            {
                return NotFound(nameof(fileTagsDto.RemoteId));
            }

            var existingTags = await _tagService.SearchByValue(fileTagsDto.Tags);

            var newTags = fileTagsDto.Tags.Where(x => !existingTags.Any(y => y.Value == x))
                                          .Select(x => new Tag() { Value = x }).ToList();

            await _tagService.CreateCollection(newTags);

            var result = await _tagService.AssignTags(file, existingTags.Union(newTags));

            return Ok(result);
        }

        [HttpGet]
        public async Task<ActionResult<Tag>> GetTagsByFile(FileMetadataTagsDto fileDto)
        {
            var file = await _fileMetadataService.SearchByRemoteId(fileDto.RemoteId);

            if (file is null)
            {
                return NotFound(nameof(fileDto.RemoteId));
            }

            var tags = await _tagService.SearchTagsByFile(file);

            return Ok(tags);
        }

        [HttpPost("search")]
        public async Task<ActionResult<FileMetadata>> SearchFilesByTags(IEnumerable<string> tags)
        {
            var existingTags = await _tagService.SearchByValue(tags);

            if (!existingTags.Any())
            {
                return NotFound(nameof(tags));
            }

            var files = await _tagService.SearchFilesByTags(existingTags);

            return Ok(files.FirstOrDefault());
        }
    }
}
