using System.Collections.Generic;

namespace FilesManager.API.Models
{
    public class FileMetadataTagsDto
    {
        public string RemoteId { get; set; }
        public string WebContentUrl { get; set; }
        public IEnumerable<string> Tags { get; set; }
    }
}
