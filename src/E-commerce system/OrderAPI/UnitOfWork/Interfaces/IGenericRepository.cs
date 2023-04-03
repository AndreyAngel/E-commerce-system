using System.Linq.Expressions;

namespace OrderAPI.UnitOfWork.Interfaces;

public interface IGenericRepository<TEntity> where TEntity : class
{
    IQueryable<TEntity> GetAll();

    TEntity? GetById(Guid Id);

    IQueryable<TEntity> Include(params Expression<Func<TEntity, object>>[] includeProperties);

    Task AddAsync(TEntity entity);

    Task UpdateAsync(TEntity entity);

    Task RemoveAsync(TEntity entity);
}
