using GbLib.BuildingBlock.Domain.Entities;
using GbLib.BuildingBlock.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace GbLib.BuildingBlock.Infrastructure.Persistence;

public class EfCommandRepository<TEntity,TKey> : ICommandRepository<TEntity> where TEntity : Entity<TKey>
{
    private readonly DbSet<TEntity> _dbSet;
    private readonly IUnitOfWork _unitOfWork;

    public EfCommandRepository(DbContext dbContext, IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
        _dbSet = dbContext.Set<TEntity>();
    }

    public async Task AddAsync(TEntity entity)
    {
        await _dbSet.AddAsync(entity);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task UpdateAsync(TEntity entity)
    {
        _dbSet.Update(entity);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task DeleteAsync(TEntity entity)
    {
        _dbSet.Remove(entity);
        await _unitOfWork.SaveChangesAsync();
    }
}