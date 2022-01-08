using System;

namespace FilesManager.Domain.Models
{
    public class FileMetadataTag
    {
        public Guid Id { get; set; }
        public FileMetadata FileMetadata { get; set; }
        public Tag Tag { get; set; }
    }
}