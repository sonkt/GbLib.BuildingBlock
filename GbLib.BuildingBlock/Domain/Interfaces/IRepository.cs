using System.Linq.Expressions;
using GbLib.BuildingBlock.Domain.Entities;

namespace GbLib.BuildingBlock.Domain.Interfaces;

public interface IRepository<TEntity> where TEntity : BaseEntity
{
    Task<TEntity?> GetByIdAsync(Guid id);
    Task<List<TEntity>> GetAllAsync();
    Task AddAsync(TEntity entity);
    void Update(TEntity entity);
    void Delete(TEntity entity);
    Task<bool> ExistsAsync(Expression<Func<TEntity, bool>> predicate);
    Task<TEntity?> GetBySpecAsync(ISpecification<TEntity> spec);
    Task<List<TEntity>> ListAsync(ISpecification<TEntity> spec);
}