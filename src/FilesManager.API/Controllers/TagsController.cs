using FilesManager.API.Filters;
using FilesManager.API.Helpers;
using FilesManager.API.Models;
using FilesManager.Application.Common.Interfaces;
using FilesManager.Application.Models;
using FilesManager.Domain.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace FilesManager.API.Controllers
{
    [ServiceFilter(typeof(AuthorizationFilter))]
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
        public async Task<ActionResult<FileMetadataSetTagsDto>> SetTags(FileMetadataTagsDto fileTagsDto)
        {
            var remoteId = fileTagsDto.WebContentUrl.GetRemoteId();

            if (remoteId is null && string.IsNullOrWhiteSpace(fileTagsDto.RemoteId))
            {
                return BadRequest(nameof(fileTagsDto.RemoteId));
            }

            if (remoteId is null)
            {
                remoteId = fileTagsDto.RemoteId;
            }

            var file = await _fileMetadataService.SearchByRemoteId(remoteId);

            if (file is null)
            {
                return NotFound(nameof(fileTagsDto.RemoteId));
            }

            var dtoTags = fileTagsDto.Tags.Where(x => !string.IsNullOrWhiteSpace(x)).Select(x => x.Trim().ToLower().RemoveAccents());

            var existingTags = await _tagService.SearchByValue(dtoTags);

            var newTags = dtoTags.Where(x => !existingTags.Any(y => y.Value == x))
                                          .Select(x => new Tag() { Value = x }).ToList();

            await _tagService.CreateCollection(newTags);

            var result = await _tagService.AssignTags(file, existingTags.Union(newTags));

            var resultDto = new FileMetadataSetTagsDto()
            {
                WebContentUrl = GoogleConstants.GenerateDownloadUrl(result.RemoteId),
                NewTags = result.NewTags,
                Tags = result.Tags
            };

            return Ok(resultDto);
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
        public async Task<ActionResult<IEnumerable<FileMetadataSearchDto>>> SearchFilesByTags([FromBody] IEnumerable<string> tags,
                                                                                              [FromQuery] int limit = 5)
        {
            try
            {
                var escapedTags = tags.Where(x => !string.IsNullOrWhiteSpace(x)).Select(x => x.Trim().ToLower().RemoveAccents()).Distinct();

                var exactTags = await _tagService.SearchByValue(escapedTags);

                var files = await _tagService.SearchFilesByTags(exactTags, limit);

                var resultDto = files.Select(x => new FileMetadataSearchDto()
                {
                    WebContentUrl = GoogleConstants.GenerateDownloadUrl(x.RemoteId),
                    Tags = x.Tags,
                    Matches = x.Matches
                });

                return Ok(resultDto);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPost("randomSearch")]
        public async Task<ActionResult<IEnumerable<FileMetadataSearchDto>>> RandomSearchFilesByTags([FromBody] IEnumerable<string> tags,
                                                                                                    [FromQuery] bool parseTags)
        {
            try
            {
                var escapedTags = tags.Where(x => !string.IsNullOrWhiteSpace(x))
                                      .Distinct()
                                      .Select(x => x.Trim()
                                                    .ToLower()
                                                    .RemoveAccents());

                var exactTags = await _tagService.SearchByValue(escapedTags);

                ParseResult parseResult = null;

                if (parseTags)
                {
                    var missingTags = escapedTags.Where(x => !exactTags.Any(y => y.Value == x));

                    parseResult = await _tagService.ParseTags(missingTags);

                    //.ToList here added to avoid a debugger error. Research needed to find out if it can be removed.
                    exactTags = exactTags.Union(parseResult.Tags).ToList();
                }

                var files = await _tagService.SearchFilesByTags(exactTags, null);

                var resultDto = files.Select(x => new FileMetadataSearchDto()
                {
                    WebContentUrl = GoogleConstants.GenerateDownloadUrl(x.RemoteId),
                    Tags = x.Tags,
                    Matches = x.Matches,
                    ParseOperations = (parseResult is null) ? new List<ParseOperationDto>() :
                                                             parseResult.ParseOperations.Select(x => new ParseOperationDto(x))
                });

                return Ok(resultDto);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpDelete("collection")]
        public async Task<ActionResult> RemoveCollection(IEnumerable<string> tags)
        {
            try
            {
                var dtoTags = tags.Where(x => !string.IsNullOrWhiteSpace(x)).Select(x => x.Trim().ToLower().RemoveAccents());

                var existingTags = await _tagService.SearchByValue(dtoTags);

                await _tagService.RemoveCollection(existingTags);

                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpGet("seedTags")]
        public async Task<ActionResult> SeedTagsFromFile()
        {
            var filesTags = new Dictionary<string, string>();

            using (var reader = new StreamReader(@"botnorrea_media.csv"))
            {
                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    var values = line.Split(',');

                    if (!string.IsNullOrWhiteSpace(values[1]))
                    {
                        if (!filesTags.ContainsKey(values[0]))
                        {
                            filesTags[values[0]] = values[1].RemoveAccents();
                        }
                        else
                        {
                            filesTags[values[0]] += " " + values[1].RemoveAccents();
                        }
                    }
                }
            }

            foreach (var key in filesTags.Keys)
            {
                var tags = filesTags[key].Split(" ", StringSplitOptions.RemoveEmptyEntries);

                var dto = new FileMetadataTagsDto()
                {
                    RemoteId = key,
                    Tags = tags
                };

                await SetTags(dto);
            }

            return Ok();
        }
    }
}
