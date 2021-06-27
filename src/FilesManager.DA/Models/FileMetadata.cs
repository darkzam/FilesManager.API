using System;
using System.Collections.Generic;
using System.Text;

namespace FilesManager.DA.Models
{
    public class FileMetadata
    {
        public Guid Id { get; set; }
        public string FileName { get; set; }
        public string Extension { get; set; }
    }
}
