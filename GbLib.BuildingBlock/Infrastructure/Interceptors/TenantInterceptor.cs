using GbLib.BuildingBlock.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace GbLib.BuildingBlock.Infrastructure.Interceptors;

public class TenantInterceptor: SaveChangesInterceptor
{
    private readonly ICurrentTenantProvider _currentTenantProvider;

    public TenantInterceptor(ICurrentTenantProvider currentTenantProvider)
    {
        _currentTenantProvider = currentTenantProvider;
    }

    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(
        DbContextEventData eventData,
        InterceptionResult<int> result,
        CancellationToken cancellationToken = default)
    {
        var context = eventData.Context;
        if (context == null) return base.SavingChangesAsync(eventData, result, cancellationToken);

        var tenantId = _currentTenantProvider.GetCurrentTenantId();

        foreach (var entry in context.ChangeTracker.Entries<ITenantEntity>().Where(e => e.State == EntityState.Added))
        {
            entry.Entity.TenantId = tenantId;
        }

        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }
}