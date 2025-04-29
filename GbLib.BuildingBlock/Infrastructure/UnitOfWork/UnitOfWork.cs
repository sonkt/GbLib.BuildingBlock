using GbLib.BuildingBlock.Domain.Entities;
using GbLib.BuildingBlock.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace GbLib.BuildingBlock.Infrastructure.UnitOfWork;

public class UnitOfWork<TDbContext> : IUnitOfWork where TDbContext : DbContext
{
    private readonly TDbContext _dbContext;
    private readonly IServiceProvider _serviceProvider;

    public UnitOfWork(TDbContext dbContext, IServiceProvider serviceProvider)
    {
        _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        _serviceProvider = serviceProvider;
    }
    public IRepository<T> Repository<T>() where T : BaseEntity
    {
        return _serviceProvider.GetRequiredService<IRepository<T>>();
    }
    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public void Dispose()
    {
        _dbContext.Dispose();
    }
}