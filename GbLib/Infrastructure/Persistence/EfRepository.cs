using System.Linq.Expressions;
using GbLib.Domain.Entities;
using GbLib.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace GbLib.Infrastructure.Persistence;

public class EfRepository<T> : IRepository<T> where T : BaseEntity
{
    protected readonly DbContext _context;
    protected readonly DbSet<T> _dbSet;

    public EfRepository(DbContext context)
    {
        _context = context;
        _dbSet = context.Set<T>();
    }

    public async Task<T?> GetByIdAsync(Guid id)
        => await _dbSet.FindAsync(id);

    public async Task<IEnumerable<T>> GetAllAsync()
        => await _dbSet.ToListAsync();

    public async Task AddAsync(T entity)
        => await _dbSet.AddAsync(entity);

    public void Update(T entity)
        => _dbSet.Update(entity);

    public void Delete(T entity)
        => _dbSet.Remove(entity);

    public async Task<bool> ExistsAsync(Expression<Func<T, bool>> predicate)
        => await _dbSet.AnyAsync(predicate);

    public async Task<T?> GetBySpecAsync(ISpecification<T> spec)
    {
        return await SpecificationEvaluator<T>
            .GetQuery(_dbSet.AsQueryable(), spec)
            .FirstOrDefaultAsync();
    }

    public async Task<List<T>> ListAsync(ISpecification<T> spec)
    {
        return await SpecificationEvaluator<T>
            .GetQuery(_dbSet.AsQueryable(), spec)
            .ToListAsync();
    }
}