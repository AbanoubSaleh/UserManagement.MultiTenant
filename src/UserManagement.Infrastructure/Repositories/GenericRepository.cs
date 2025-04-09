using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using UserManagement.Application.Common.Interfaces.Repositories;
using UserManagement.Domain.Common;
using UserManagement.Persistence.Context;

namespace UserManagement.Infrastructure.Repositories;

public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : class, IEntity
{
    protected readonly ApplicationDbContext _context;
    protected readonly DbSet<TEntity> _dbSet;

    public GenericRepository(ApplicationDbContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _dbSet = context.Set<TEntity>();
    }

    public virtual async Task<TEntity?> GetByIdAsync(Guid id)
    {
        return await _dbSet.FindAsync(id);
    }

    public virtual async Task<IEnumerable<TEntity>> GetAllAsync()
    {
        return await _dbSet.ToListAsync();
    }

    public virtual async Task<IEnumerable<TEntity>> FindAsync(Expression<Func<TEntity, bool>> predicate)
    {
        return await _dbSet.Where(predicate).ToListAsync();
    }

    public virtual async Task<TEntity> AddAsync(TEntity entity)
    {
        if (entity == null) throw new ArgumentNullException(nameof(entity));     
        await _dbSet.AddAsync(entity);
        return entity;
    }

    public virtual void Update(TEntity entity)
    {
        if (entity == null) throw new ArgumentNullException(nameof(entity));
                
        // Attach if not tracked
        if (_context.Entry(entity).State == EntityState.Detached)
        {
            _dbSet.Attach(entity);
        }
        
        _context.Entry(entity).State = EntityState.Modified;
    }

    public virtual void Delete(TEntity entity)
    {
        if (entity == null) throw new ArgumentNullException(nameof(entity));     
        Update(entity);
    }

    public virtual IQueryable<TEntity> Query()
    {
        return _dbSet.AsNoTracking();
    }

    public async Task<bool> ExistsAsync(Guid id)
    {
        return await _dbSet.AnyAsync(e => e.Id == id);
    }
    
    public async Task<int> SaveChangesAsync()
    {
        return await _context.SaveChangesAsync();
    }
}