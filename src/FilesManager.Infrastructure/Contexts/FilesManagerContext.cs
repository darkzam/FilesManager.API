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
        public DbSet<Tag> Tags { get; set; }
        public DbSet<FileMetadataTag> FileMetadataTags { get; set; }
        public DbSet<Category> Category { get; set; }
        public DbSet<Setting> Settings { get; set; }
    }
}
