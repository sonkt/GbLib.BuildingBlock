using GbLib.BuildingBlock.Domain.Entities;
using GbLib.BuildingBlock.Domain.Interfaces;
using GbLib.BuildingBlock.Domain.Specifications;
using Microsoft.EntityFrameworkCore;

namespace GbLib.BuildingBlock.Infrastructure.Persistence;

public class EfQueryRepository<TEntity,TKey> : IQueryRepository<TEntity> where TEntity : Entity<TKey>
{
    private readonly DbSet<TEntity> _dbSet;

    public EfQueryRepository(DbContext dbContext)
    {
        _dbSet = dbContext.Set<TEntity>();
    }

    public async Task<TEntity?> GetByIdAsync(Guid id)
    {
        return await _dbSet.FindAsync(id);
    }

    public async Task<List<TEntity>> ListAsync(BaseSpecification<TEntity> spec)
    {
        return await ApplySpecification(spec).ToListAsync();
    }

    public async Task<(List<TEntity> Items, int TotalCount)> ListAsyncWithPaging(BaseSpecification<TEntity> spec)
    {
        var query = ApplySpecification(spec);
        var totalCount = await query.CountAsync();
        var items = await query.Skip(spec.Skip ?? 0).Take(spec.Take ?? int.MaxValue)
            .ToListAsync(); // Use Skip/Take from the specification
        return (items, totalCount);
    }

    public async Task<int> CountAsync(BaseSpecification<TEntity> spec)
    {
        return await ApplySpecification(spec).CountAsync();
    }

    private IQueryable<TEntity> ApplySpecification(BaseSpecification<TEntity> spec)
    {
        return SpecificationEvaluator<TEntity>.GetQuery(_dbSet.AsQueryable(), spec);
    }
}
