using OnePlatform.Clearing.Domain.Enums;

namespace OnePlatform.Clearing.Domain.DomainEvents;

public sealed record PaymentReceivedDomainEvent(Guid paymentId, PaymentEventType eventType, DateTime receivedAt, string? details) : IDomainEvent
{
  
}