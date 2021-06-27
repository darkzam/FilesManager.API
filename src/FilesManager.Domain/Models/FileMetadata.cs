using System;

namespace FilesManager.Domain.Models
{
    public class FileMetadata
    {
        public Guid Id { get; set; }
        public string FileName { get; set; }
        public string Extension { get; set; }
    }
}
