namespace GbLib.BuildingBlock.Domain.Interfaces;

public interface ICurrentTenantProvider
{
    Guid GetCurrentTenantId();
}