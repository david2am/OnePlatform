namespace OnePlatform.Clearing.Domain.Entities;

public abstract class Entity : IEquatable<Entity>
{
  protected Entity(Guid id)
  {
    Id = id;
  }

  public Guid Id { get; private init; }

  public static bool operator ==(Entity? first, Entity? second)
  {
    if (first is null && second is null) return true;
    if (first is null || second is null) return false;
    return first.Equals(second);
  }

  public static bool operator !=(Entity? first, Entity? second)
    => !(first == second);

  public override bool Equals(object? obj)
    => obj is Entity other && Equals(other);

  public bool Equals(Entity? other)
  {
    if (other is null) return false;
    if (ReferenceEquals(this, other)) return true;
    if (other.GetType() != GetType()) return false;

    return other.Id == Id;
  }

  public override int GetHashCode() => Id.GetHashCode();
}