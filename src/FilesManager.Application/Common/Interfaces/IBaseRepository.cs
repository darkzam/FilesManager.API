using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace FilesManager.Application.Common.Interfaces
{
    public interface IBaseRepository<TEntity> where TEntity : class
    {
        Task<IEnumerable<TEntity>> GetAll();
        Task<TEntity> Find(Guid id);
        Task<IEnumerable<TEntity>> FindCollection(IEnumerable<Guid> ids);
        Task<IEnumerable<TEntity>> SearchBy(Expression<Func<TEntity, bool>> predicate);
        TEntity Create(TEntity entity);
        void CreateCollection(IEnumerable<TEntity> entities);
        void Update(TEntity entity);
        void UpdateCollection(IEnumerable<TEntity> entities);
        void Remove(TEntity entity);
        void RemoveCollection(IEnumerable<TEntity> entities);
    }
}
