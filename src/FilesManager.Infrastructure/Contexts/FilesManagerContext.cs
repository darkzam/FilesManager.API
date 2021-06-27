using FilesManager.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace FilesManager.Infrastructure.Contexts
{
    public class FilesManagerContext : DbContext
    {
        public FilesManagerContext(DbContextOptions<FilesManagerContext> dbContextOptions) : base(dbContextOptions)
        {
        }

        public DbSet<FileMetadata> FileMetadata { get; set; }
    }
}
