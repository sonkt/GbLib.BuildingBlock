using System.ComponentModel.DataAnnotations.Schema;
using GbLib.BuildingBlock.Domain.Interfaces;

namespace GbLib.BuildingBlock.Domain.Entities;

public abstract class Entity : Entity<Guid>, IEntity
{
}

public abstract class Entity<TKey> : IEntity<TKey>
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public virtual TKey Id { get; set; } = default!;
    public bool IsDeleted { get; set; } = false;
    public DateTime? DeletedAt { get; set; }
    public Guid? DeletedBy { get; set; }

    public override bool Equals(object? obj)
    {
        if (obj is not Entity<TKey> other)
            return false;


        if (ReferenceEquals(this, other))
            return true;


        if (GetRealType() != other.GetRealType())
            return false;


        if (Id.Equals(default) || other.Id.Equals(default))
            return false;


        return Id.Equals(other.Id);
    }

    public static bool operator ==(Entity<TKey> a, Entity<TKey> b)
    {
        if (a is null && b is null)
            return true;

        if (a is null || b is null)
            return false;

        return a.Equals(b);
    }

    public static bool operator !=(Entity<TKey> a, Entity<TKey> b)
    {
        return !(a == b);
    }

    public override int GetHashCode()
    {
        return (GetRealType().ToString() + Id.ToString()).GetHashCode();
    }

    private Type GetRealType()
    {
        return this.GetType();
    }
}

public abstract class AggregateRoot : Entity
{
}

public abstract class AuditableAggregateRoot : AuditableEntity
{
}

public abstract class AggregateRoot<TKey> : Entity<TKey>
{
}
public abstract class AuditableAggregateRoot<TKey> : AuditableEntity<TKey>
{
}

public abstract class AuditableEntity : Entity,IHasAudit
{
    public DateTime CreatedAt { get; set; }= DateTime.UtcNow;
    public string CreatedBy { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public string? UpdatedBy { get; set; }
    public DateTime? DeletedAt { get; set; }
    public string? DeletedBy { get; set; }
}

public abstract class AuditableEntity<TKey> : Entity<TKey>,IHasAudit
{
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public DateTime? DeletedAt { get; set; }
    public string? DeletedBy { get; set; }
    public string CreatedBy { get; set; }
    public string? UpdatedBy { get; set; }
}