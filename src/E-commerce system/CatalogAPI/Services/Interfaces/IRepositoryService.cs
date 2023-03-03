using System.Linq.Expressions;

namespace CatalogAPI.Services.Interfaces;

public interface IRepositoryService<TEntity> where TEntity : class
{
    IEnumerable<TEntity> GetAll();

    TEntity GetById(int id);

    TEntity GetByName(string name);

    TEntity GetByFilter(Func<TEntity, bool> predicate);

    IEnumerable<TEntity> GetListByFilter(Func<TEntity, bool> predicate);

    TEntity GetWithInclude(Func<TEntity, bool> predicate, params Expression<Func<TEntity, object>>[] includeProperties);

    IEnumerable<TEntity> GetAllWithInclude(params Expression<Func<TEntity, object>>[] includeProperties);

    IEnumerable<TEntity> GetListWithInclude(Func<TEntity, bool> predicate,
        params Expression<Func<TEntity, object>>[] includeProperties);

    Task AddAsync(TEntity entity);

    Task UpdateAsync(TEntity entity);

    Task RemoveAsync(int id);
}
