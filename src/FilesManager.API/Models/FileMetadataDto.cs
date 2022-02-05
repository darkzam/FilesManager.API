﻿using System.Collections.Generic;

namespace FilesManager.API.Models
{
    public class FileMetadataDto
    {
        public string WebContentUrl { get; set; }
        public IEnumerable<string> Tags { get; set; }
    }
}
