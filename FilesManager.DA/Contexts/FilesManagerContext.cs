using FilesManager.DA.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System;
using System.Collections.Generic;
using System.Text;

namespace FilesManager.DA.Contexts
{
    public class FilesManagerContext : DbContext
    {
        public FilesManagerContext(DbContextOptions<FilesManagerContext> dbContextOptions) : base(dbContextOptions)
        {
        }

        public DbSet<FileMetadata> FileMetadata { get; set; }
    }
}
