
using OnePlatform.Clearing.Shared.Domain.DomainEvents;
using OnePlatform.Clearing.Shared.Domain.Entities;

namespace OnePlatform.Clearing.Tests.Shared.Domain.Entities;

public class AggregateRootTests
{
    [Fact]
    public void GetDomainEvents_ShouldReturnEmptyCollection_WhenNoEventsWereRaised()
    {
        // Arrange
        var aggregate = new TestAggregate(Guid.NewGuid());

        // Act
        var events = aggregate.GetDomainEvents();

        // Assert
        Assert.Empty(events);
    }

    [Fact]
    public void GetDomainEvents_ShouldReturnRaisedEvents()
    {
        // Arrange
        var aggregate = new TestAggregate(Guid.NewGuid());
        var domainEvent = new TestDomainEvent();

        // Act
        aggregate.Raise(domainEvent);

        var events = aggregate.GetDomainEvents();

        // Assert
        Assert.Single(events);
        Assert.Same(domainEvent, events.Single());
    }

    [Fact]
    public void GetDomainEvents_ShouldReturnAllRaisedEvents_InOrder()
    {
        // Arrange
        var aggregate = new TestAggregate(Guid.NewGuid());

        var firstEvent = new TestDomainEvent();
        var secondEvent = new TestDomainEvent();

        // Act
        aggregate.Raise(firstEvent);
        aggregate.Raise(secondEvent);

        var events = aggregate.GetDomainEvents();

        // Assert
        Assert.Collection(
            events,
            item => Assert.Same(firstEvent, item),
            item => Assert.Same(secondEvent, item)
        );
    }

    [Fact]
    public void GetDomainEvents_ShouldReturnSnapshot_OfInternalEvents()
    {
        // Arrange
        var aggregate = new TestAggregate(Guid.NewGuid());
        var firstEvent = new TestDomainEvent();

        aggregate.Raise(firstEvent);

        var events = aggregate.GetDomainEvents();

        // Act
        aggregate.Raise(new TestDomainEvent());

        // Assert
        Assert.Single(events);
        Assert.Equal(2, aggregate.GetDomainEvents().Count);
    }

    [Fact]
    public void ClearDomainEvents_ShouldRemoveAllRaisedEvents()
    {
        // Arrange
        var aggregate = new TestAggregate(Guid.NewGuid());

        aggregate.Raise(new TestDomainEvent());
        aggregate.Raise(new TestDomainEvent());

        // Act
        aggregate.ClearDomainEvents();

        // Assert
        Assert.Empty(aggregate.GetDomainEvents());
    }

    [Fact]
    public void ClearDomainEvents_ShouldAllowRaisingNewEvents()
    {
        // Arrange
        var aggregate = new TestAggregate(Guid.NewGuid());

        aggregate.Raise(new TestDomainEvent());
        aggregate.ClearDomainEvents();

        var newEvent = new TestDomainEvent();

        // Act
        aggregate.Raise(newEvent);

        var events = aggregate.GetDomainEvents();

        // Assert
        Assert.Single(events);
        Assert.Same(newEvent, events.Single());
    }

    [Fact]
    public void ClearDomainEvents_ShouldNotModifyPreviouslyReturnedSnapshot()
    {
        // Arrange
        var aggregate = new TestAggregate(Guid.NewGuid());
        aggregate.Raise(new TestDomainEvent());

        var events = aggregate.GetDomainEvents();

        // Act
        aggregate.ClearDomainEvents();

        // Assert
        Assert.Single(events);
        Assert.Empty(aggregate.GetDomainEvents());
    }

    private sealed class TestAggregate(Guid id) : AggregateRoot<Guid>(id)
    {
        public void Raise(IDomainEvent domainEvent)
            => RaiseDomainEvent(domainEvent);
    }

    private sealed record TestDomainEvent : IDomainEvent;
}
