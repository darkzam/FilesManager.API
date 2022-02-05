using System.Collections.Generic;

namespace FilesManager.Application.Models
{
    public class FileSetTagsModel
    {
        public string RemoteId { get; set; }
        public IEnumerable<string> NewTags { get; set; }
        public IEnumerable<string> Tags { get; set; }
    }
}
