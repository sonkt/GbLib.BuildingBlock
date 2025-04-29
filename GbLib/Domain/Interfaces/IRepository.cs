using System.Linq.Expressions;
using GbLib.Domain.Entities;

namespace GbLib.Domain.Interfaces;

public interface IRepository<TEntity> where TEntity : BaseEntity
{
    Task<TEntity?> GetByIdAsync(Guid id);
    Task<IEnumerable<TEntity>> GetAllAsync();
    Task AddAsync(TEntity entity);
    void Update(TEntity entity);
    void Delete(TEntity entity);
    Task<bool> ExistsAsync(Expression<Func<TEntity, bool>> predicate);
    Task<TEntity?> GetBySpecAsync(ISpecification<TEntity> spec);
    Task<List<TEntity>> ListAsync(ISpecification<TEntity> spec);
}