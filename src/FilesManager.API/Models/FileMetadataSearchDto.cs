using System.Collections.Generic;

namespace FilesManager.API.Models
{
    public class FileMetadataSearchDto
    {
        public string WebContentUrl { get; set; }
        public IEnumerable<string> Tags { get; set; }
        public int Matches { get; set; }
        public IEnumerable<ParseOperationDto> ParseOperations { get; set; }
    }
}
