using GbLib.BuildingBlock.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace GbLib.BuildingBlock.Infrastructure.Interceptors;

public class AuditInterceptor:SaveChangesInterceptor
{
    private readonly ICurrentUserProvider _currentUserProvider;

    public AuditInterceptor(ICurrentUserProvider currentUserProvider)
    {
        _currentUserProvider = currentUserProvider;
    }

    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(
        DbContextEventData eventData,
        InterceptionResult<int> result,
        CancellationToken cancellationToken = default)
    {
        var context = eventData.Context;
        if (context == null) return base.SavingChangesAsync(eventData, result, cancellationToken);

        var userId = _currentUserProvider.GetCurrentUserId();
        var now = DateTime.UtcNow;

        foreach (var entry in context.ChangeTracker.Entries<IHasAudit>())
        {
            switch (entry.State)
            {
                case EntityState.Added:
                    entry.Entity.CreatedAt = now;
                    entry.Entity.CreatedBy = userId;
                    break;
                case EntityState.Modified:
                    entry.Entity.UpdatedAt = now;
                    entry.Entity.UpdatedBy = userId;
                    break;
                case EntityState.Deleted:
                    entry.Entity.DeletedAt = now;
                    entry.Entity.DeletedBy = userId;
                    break;
            }
        }

        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }
}