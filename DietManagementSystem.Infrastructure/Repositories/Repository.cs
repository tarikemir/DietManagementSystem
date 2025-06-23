using DietManagementSystem.Application.Common;
using DietManagementSystem.Persistence;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace DietManagementSystem.Infrastructure.Repositories;

public class Repository<T> : IRepository<T> where T : class
{
    protected readonly DietManagementSystemDbContext _context;

    protected readonly DbSet<T> _dbSet;

    public Repository(DietManagementSystemDbContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _dbSet = _context.Set<T>();
    }

    public async Task<T?> GetByIdAsync(Guid id)
    {
        return await _dbSet.FindAsync(id);
    }

    public IQueryable<T> GetAllAsync()
    {
        return _dbSet.AsNoTracking();
    }

    public IQueryable<T> FindAsync(Expression<Func<T, bool>> predicate)
    {
        return _dbSet.Where(predicate).AsNoTracking();
    }

    public async Task AddAsync(T entity)
    {
        if (entity == null) throw new ArgumentNullException(nameof(entity));
        await _dbSet.AddAsync(entity);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(T entity)
    {
        if (entity == null) throw new ArgumentNullException(nameof(entity));
        _dbSet.Update(entity);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        var entity = await GetByIdAsync(id);
        if (entity == null) throw new KeyNotFoundException($"Entity with ID {id} not found.");
        _dbSet.Remove(entity);
        await _context.SaveChangesAsync();
    }
    public async Task<T> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate)
    {
        return await _dbSet.AsNoTracking().FirstOrDefaultAsync(predicate);
    }

    public IQueryable<T> Query()
    {
        return _dbSet.AsQueryable();
    }
}
