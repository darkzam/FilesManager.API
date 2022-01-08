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

        [HttpPost("")]
        public async Task<ActionResult<IEnumerable<FileMetadataTag>>> Create(FileMetadataTagsDto fileTagsDto)
        {
            var file = await _fileMetadataService.SearchByRemoteId(fileTagsDto.RemoteId);

            if (file is null)
            {
                return NotFound(nameof(fileTagsDto.RemoteId));
            }

            var tags = fileTagsDto.Tags.Select(x => new Tag() { Value = x });

            var createdTags = await _tagService.CreateCollection(tags);

            var result = await _tagService.AssignTags(file, createdTags);

            return Ok(result);
        }
    }
}
