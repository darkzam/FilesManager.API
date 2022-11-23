using FilesManager.Application.Common.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace FilesManager.Infrastructure.Repositories
{
    public abstract class BaseRepository<TEntity> : IBaseRepository<TEntity> where TEntity : class
    {
        protected readonly DbContext _dbContext;
        protected BaseRepository(DbContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        //We would like to create an async method whenever 
        //we are executing long running instructions
        //Calling Add(Entity) is not a costly operation
        //it's less than 3 instructions O(1)
        //so it would be a good idea to make it synchronous
        //and return a Value and not a Task<T>
        public virtual TEntity Create(TEntity entity)
        {
            var result = _dbContext.Set<TEntity>().Add(entity);

            return result.Entity;
        }

        //AddRange might be an expensive operation in the worst case O(n)
        //As our parameter list won't be bigger than 10 elements
        //it's complexity will be O(10) in worst case which is pretty fast
        //that's why we'll make this method synchronous
        public virtual void CreateCollection(IEnumerable<TEntity> entities)
        {
            _dbContext.Set<TEntity>().AddRange(entities);
        }

        public virtual async Task<TEntity> Find(Guid id)
        {
            return await _dbContext.Set<TEntity>().FindAsync(id);
        }

        public virtual async Task<IEnumerable<TEntity>> FindCollection(IEnumerable<Guid> ids)
        {
            var param = Expression.Parameter(typeof(TEntity));
            var property = Expression.Property(param, "Id");
            var constantList = Expression.Constant(ids.ToList());

            var method = typeof(List<Guid>).GetMethod(nameof(Enumerable.Contains),
                                                      new Type[] { typeof(Guid) });

            var containsCall = Expression.Call(constantList,
                                               method,
                                               property);

            var condition = Expression.Lambda<Func<TEntity, bool>>(containsCall, param);

            return await _dbContext.Set<TEntity>().Where(condition).ToListAsync();
        }

        //Fetching the data from the DB and deserializing in a list
        //is an expensive operation O(n)
        //that's why we are gonna make it asynchronous by returning a task
        //now this implementation is already wrapped inside the method
        //ToListAsync. We must then add async to be able to await
        //for the result of this operation and finally return the files list
        public virtual async Task<IEnumerable<TEntity>> GetAll()
        {
            return await _dbContext.Set<TEntity>().ToListAsync();
        }

        //Set EntityState.Deleted
        //inexpensive O(1)
        //then wrap it in a synchronous method
        public virtual void Remove(TEntity entity)
        {
            _dbContext.Set<TEntity>().Remove(entity);
        }

        //it's O(n) for n <= 10
        //it's still pretty inexpensive as we have 10 as upper boundary
        //In the future I would like to test 
        //for a bigger N value and see if RemoveRange doesn't perform well
        public virtual void RemoveCollection(IEnumerable<TEntity> entities)
        {
            _dbContext.Set<TEntity>().RemoveRange(entities);
        }

        public virtual async Task<IEnumerable<TEntity>> SearchBy(Expression<Func<TEntity, bool>> predicate)
        {
            return await _dbContext.Set<TEntity>().Where(predicate).ToListAsync();
        }

        //It just marks entities with EntityState.Modified state
        //and sets the rest of the property
        //all of it is O(1)
        public virtual void Update(TEntity entity)
        {
            _dbContext.Set<TEntity>().Update(entity);
        }

        //UpdateRange then is O(n) but as n <= 10 
        // O(10) is not an expensive operation
        // we might need to test if actually O(10) might slow down
        //the whole operation
        public virtual void UpdateCollection(IEnumerable<TEntity> entities)
        {
            _dbContext.Set<TEntity>().UpdateRange(entities);
        }
    }
}
