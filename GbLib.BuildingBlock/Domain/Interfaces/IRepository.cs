using System.Linq.Expressions;
using GbLib.BuildingBlock.Domain.Entities;
using GbLib.BuildingBlock.Domain.Specifications;

namespace GbLib.BuildingBlock.Domain.Interfaces;

public interface IQueryRepository<TEntity> where TEntity : IEntity
{
    Task<TEntity?> GetByIdAsync(Guid id);
    Task<List<TEntity>> ListAsync(BaseSpecification<TEntity> spec); // Use BaseSpecification directly
    Task<(List<TEntity> Items, int TotalCount)> ListAsyncWithPaging(BaseSpecification<TEntity> spec); // Use BaseSpecification for paging as well
    Task<int> CountAsync(BaseSpecification<TEntity> spec);
}

public interface ICommandRepository<in TEntity> where TEntity : IEntity
{
    Task AddAsync(TEntity entity);
    Task UpdateAsync(TEntity entity);
    Task DeleteAsync(TEntity entity);
}
