namespace OnePlatform.Clearing.Shared.Domain.Entities;

public abstract class Entity<TId> : IEquatable<Entity<TId>> where TId : notnull
{
  protected Entity(TId id)
  {
    Id = id;
  }

  public TId Id { get; private init; }

  public static bool operator ==(Entity<TId>? first, Entity<TId>? second)
  {
    if (first is null && second is null) return true;
    if (first is null || second is null) return false;
    return first.Equals(second);
  }

  public static bool operator !=(Entity<TId>? first, Entity<TId>? second)
    => !(first == second);

  public override bool Equals(object? obj)
    => obj is Entity<TId> other && Equals(other);

  public bool Equals(Entity<TId>? other)
  {
    if (other is null) return false;
    if (ReferenceEquals(this, other)) return true;
    if (other.GetType() != GetType()) return false;

    return EqualityComparer<TId>.Default.Equals(other.Id, Id);
  }

  public override int GetHashCode() => Id.GetHashCode();
}