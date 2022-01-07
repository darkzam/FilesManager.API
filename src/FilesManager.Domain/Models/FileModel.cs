using System.IO;

namespace FilesManager.Domain.Models
{
    public class FileModel
    {
        public string Name { get; set; }
        public string MimeType { get; set; }
        public MemoryStream Content { get; set; }
    }
}
