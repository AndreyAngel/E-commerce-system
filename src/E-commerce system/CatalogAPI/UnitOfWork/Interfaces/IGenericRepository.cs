using System.Linq.Expressions;

namespace CatalogAPI.UnitOfWork.Interfaces;

public interface IGenericRepository<TEntity> where TEntity : class
{
    IEnumerable<TEntity> GetAll();

    TEntity? GetById(int Id);

    IQueryable<TEntity> Include(params Expression<Func<TEntity, object>>[] includeProperties);

    Task AddAsync(TEntity entity);

    Task UpdateAsync(TEntity entity);

    Task RemoveAsync(TEntity entity);
}
