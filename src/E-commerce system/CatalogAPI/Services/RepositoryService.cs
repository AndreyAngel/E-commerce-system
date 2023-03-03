using CatalogAPI.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using CatalogAPI.Models.DataBase;

namespace CatalogAPI.Services;

public class RepositoryService<TEntity>: IRepositoryService<TEntity> where TEntity : class, IEntity
{
    private readonly Context _context;
    private readonly DbSet<TEntity> _db;
    public RepositoryService(Context context)
    {
        _context = context;
        _db = context.Set<TEntity>();
    }

    public IEnumerable<TEntity> GetAll()
    {
        return _db.AsNoTracking().ToList();
    }

    public TEntity GetById(int id)
    {
        return _db.AsNoTracking().SingleOrDefault(x => x.Id == id);
    }

    public TEntity GetByName(string name)
    {
        return _db.AsNoTracking().SingleOrDefault(x => x.Name == name);
    }

    public TEntity GetByFilter(Func<TEntity, bool> predicate)
    {
        return _db.AsNoTracking().SingleOrDefault(predicate);
    }

    public IEnumerable<TEntity> GetListByFilter(Func<TEntity, bool> predicate)
    {
        return _db.AsNoTracking().Where(predicate);
    }

    public TEntity GetWithInclude(Func<TEntity, bool> predicate,
        params Expression<Func<TEntity, object>>[] includeProperties)
    {
        var query = Include(includeProperties);
        return query.SingleOrDefault(predicate);
    }

    public IEnumerable<TEntity> GetAllWithInclude(params Expression<Func<TEntity, object>>[] includeProperties)
    {
        return Include(includeProperties);
    }

    public IEnumerable<TEntity> GetListWithInclude(Func<TEntity, bool> predicate,
        params Expression<Func<TEntity, object>>[] includeProperties)
    {
        var query = Include(includeProperties);
        return query.Where(predicate);
    }

    public async Task AddAsync(TEntity entity)
    {
        await _db.AddAsync(entity);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(TEntity entity)
    {
        await Task.Run( () => _db.Update(entity));
        await _context.SaveChangesAsync();
    }

    public async Task RemoveAsync(int id)
    {
        TEntity entity = _db.Single(x => x.Id == id);
        await Task.Run( () => _db.Remove(entity));
        await _context.SaveChangesAsync();
    }

    private IQueryable<TEntity> Include(params Expression<Func<TEntity, object>>[] includeProperties)
    {
        IQueryable<TEntity> query = _db;
        return includeProperties
            .Aggregate(query, (current, includeProperty) => current.Include(includeProperty));
    }
}
