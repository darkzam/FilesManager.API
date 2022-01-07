using System;

namespace FilesManager.Domain.Models
{
    public class FileMetadata
    {
        public Guid Id { get; set; }
        public string FileName { get; set; }
        public string MimeType { get; set; }
        public string RemoteId { get; set; }
    }
}
