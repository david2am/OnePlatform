using OnePlatform.Clearing.Domain.DomainEvents;
namespace OnePlatform.Clearing.Domain.Entities;

public abstract class AggregateRoot<TId> : Entity<TId> where TId : notnull
{
  private readonly List<IDomainEvent> _domainEvents = new();
  protected AggregateRoot(TId id) : base(id)
  {
  }

  public IReadOnlyCollection<IDomainEvent> GetDomainEvents() => _domainEvents.ToList();
  public void ClearDomainEvents() => _domainEvents.Clear();
 
  protected void RaiseDomainEvent(IDomainEvent domainEvent)
  {
    _domainEvents.Add(domainEvent);
  }
}