using GbLib.BuildingBlock.Domain.Entities;

namespace GbLib.BuildingBlock.Domain.Interfaces;

public interface IUnitOfWork : IDisposable
{
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    IRepository<T> Repository<T>() where T : BaseEntity;
}