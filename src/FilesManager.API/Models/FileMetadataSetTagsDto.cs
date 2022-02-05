using System.Collections.Generic;

namespace FilesManager.API.Models
{
    public class FileMetadataSetTagsDto
    {
        public string WebContentUrl { get; set; }
        public IEnumerable<string> Tags { get; set; }
        public IEnumerable<string> NewTags { get; set; }
    }
}
