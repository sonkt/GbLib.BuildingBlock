using System.Linq.Expressions;
using GbLib.BuildingBlock.Domain.Entities;
using GbLib.BuildingBlock.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace GbLib.BuildingBlock.Infrastructure.Persistence;

public class EfRepository<T> : IRepository<T> where T : BaseEntity
{
    private readonly DbSet<T> _entities;

    public EfRepository(DbContext dbContext)
    {
        _entities = dbContext.Set<T>();
    }

    public async Task<T?> GetByIdAsync(Guid id)
    {
        ValidateId(id);
        return await _entities.FindAsync(id);
    }

    public async Task<List<T>> GetAllAsync()
    {
        return await _entities.ToListAsync();
    }

    public async Task AddAsync(T entity)
    {
        await _entities.AddAsync(entity);
    }

    public void Update(T entity)
    {
        _entities.Update(entity);
    }

    public void Delete(T entity)
    {
        _entities.Remove(entity);
    }

    public async Task<bool> ExistsAsync(Expression<Func<T, bool>> predicate)
    {
        return await _entities.AnyAsync(predicate);
    }

    public async Task<T?> GetBySpecAsync(ISpecification<T> spec)
    {
        return await ApplySpecification(spec).FirstOrDefaultAsync();
    }

    public async Task<List<T>> ListAsync(ISpecification<T> spec)
    {
        return await ApplySpecification(spec).ToListAsync();
    }

    private IQueryable<T> ApplySpecification(ISpecification<T> spec)
    {
        return SpecificationEvaluator<T>.GetQuery(_entities.AsQueryable(), spec);
    }

    private void ValidateId(Guid id)
    {
        if (id == Guid.Empty)
        {
            throw new ArgumentException("ID cannot be empty.", nameof(id));
        }
    }
}