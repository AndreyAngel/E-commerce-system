using CatalogAPI.Models.DataBase;
using CatalogAPI.UnitOfWork.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
namespace CatalogAPI.UnitOfWork;

public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : class, IEntity
{
    private readonly Context _context;
    private readonly DbSet<TEntity> _db;

    public GenericRepository(Context context)
    {
        _context = context;
        _db = context.Set<TEntity>();
    }
    public IEnumerable<TEntity> GetAll()
    {
        return _db.AsNoTracking();
    }

    public IQueryable<TEntity> Include(params Expression<Func<TEntity, object>>[] includeProperties)
    {
        IQueryable<TEntity> query = _db.AsNoTracking();
        return includeProperties
            .Aggregate(query, (current, includeProperty) => current.Include(includeProperty));
    }

    public TEntity? GetById(int Id)
    {
        var entity = _db.Find(Id);

        if (entity == null)
        {
            return null;
        }

        _context.Entry(entity).State = EntityState.Detached;

        return entity;
    }

    public async Task AddAsync(TEntity entity)
    {

        await _db.AddAsync(entity);
    }

    public async Task UpdateAsync(TEntity entity)
    {
        await Task.Run(() => _db.Update(entity));
    }

    public async Task RemoveAsync(TEntity entity)
    {
        await Task.Run(() => _db.Remove(entity));
    }
}
