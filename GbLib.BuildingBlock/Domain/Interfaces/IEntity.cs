namespace GbLib.BuildingBlock.Domain.Interfaces;

public interface IEntity<out TKey> : IEntity
{
    TKey Id { get; }
}
public interface IEntity
{
}

