using FilesManager.Application.Common.Interfaces;
using FilesManager.Domain.Models;
using FilesManager.Infrastructure.Contexts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FilesManager.Infrastructure.Repositories
{
    public class FileMetadataRepository : IFileMetadataRepository
    {
        private readonly FilesManagerContext _filesManagerContext;
        public FileMetadataRepository(FilesManagerContext filesManagerContext)
        {
            _filesManagerContext = filesManagerContext ?? throw new ArgumentNullException(nameof(filesManagerContext));
        }

        //Fetching the data from the DB and deserializing in a list
        //is an expensive operation O(n)
        //that's why we are gonna make it asynchronous by returning a task
        //now this implementation is already wrapped inside the method
        //ToListAsync. We must then add async to be able to await
        //for the result of this operation and finally return the files list
        public async Task<IEnumerable<FileMetadata>> GetAll()
        {
            return await _filesManagerContext.FileMetadata.ToListAsync();
        }

        //We would like to create an async method whenever 
        //we are executing long running instructions
        //Calling Add(Entity) is not a costly operation
        //it's less than 3 instructions O(1)
        //so it would be a good idea to make it synchronous
        //and return a Value and not a Task<T>
        public FileMetadata Create(FileMetadata fileMetadata)
        {
            var result = _filesManagerContext.FileMetadata.Add(fileMetadata);

            return result.Entity;
        }


        //AddRange might be an expensive operation in the worst case O(n)
        //As our parameter list won't be bigger than 10 elements
        //it's complexity will be O(10) in worst case which is pretty fast
        //that's why we'll make this method synchronous
        public void CreateCollection(IEnumerable<FileMetadata> filesMetadata)
        {
            _filesManagerContext.FileMetadata.AddRange(filesMetadata);
        }

        public async Task<FileMetadata> Find(Guid id)
        {
            return await _filesManagerContext.FileMetadata.FindAsync(id);
        }

        public async Task<IEnumerable<FileMetadata>> FindCollection(IEnumerable<Guid> ids)
        {
            return await _filesManagerContext.FileMetadata.Where(fileMetadata => ids.Contains(fileMetadata.Id)).ToListAsync();
        }

        //It just marks entities with EntityState.Modified state
        //and sets the rest of the property
        //all of it is O(1)
        public void Update(FileMetadata fileMetadata)
        {
            _filesManagerContext.FileMetadata.Update(fileMetadata);
        }

        //UpdateRange then is O(n) but as n <= 10 
        // O(10) is not an expensive operation
        // we might need to test if actually O(10) might slow down
        //the whole operation
        public void UpdateCollection(IEnumerable<FileMetadata> filesMetadata)
        {
            _filesManagerContext.FileMetadata.UpdateRange(filesMetadata);
        }

        //Set EntityState.Deleted
        //inexpensive O(1)
        //then wrap it in a synchronous method
        public void Remove(FileMetadata fileMetadata)
        {
            _filesManagerContext.FileMetadata.Remove(fileMetadata);
        }

        //it's O(n) for n <= 10
        //it's still pretty inexpensive as we have 10 as upper boundary
        //In the future I would like to test 
        //for a bigger N value and see if RemoveRange doesn't perform well
        public void RemoveCollection(IEnumerable<FileMetadata> filesMetadata)
        {
            _filesManagerContext.FileMetadata.RemoveRange(filesMetadata);
        }
    }
}
