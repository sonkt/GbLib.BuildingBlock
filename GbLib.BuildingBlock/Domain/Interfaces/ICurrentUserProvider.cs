namespace GbLib.BuildingBlock.Domain.Interfaces;

public interface ICurrentUserProvider
{
    string GetCurrentUserId();
    void SetCurrentUserId(string userId);
}