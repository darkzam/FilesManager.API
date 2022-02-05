using System.Collections.Generic;

namespace FilesManager.Application.Models
{
    public class FileSearchModel
    {
        public string RemoteId { get; set; }
        public IEnumerable<string> Tags { get; set; }
        public int Matches { get; set; }
    }
}
