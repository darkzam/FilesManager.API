using FilesManager.Domain.Models;
using System.Collections.Generic;

namespace FilesManager.Application.Models
{
    public class ParseResult
    {
        public IEnumerable<Tag> Tags { get; set; }
        public IEnumerable<ParseOperation> ParseOperations { get; set; }
    }
}