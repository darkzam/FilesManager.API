using FilesManager.Domain.Models;

namespace FilesManager.Application.Models
{
    public class FileFrequency
    {
        public FileMetadata File { get; set; }
        public int Frequency { get; set; }
    }
}
