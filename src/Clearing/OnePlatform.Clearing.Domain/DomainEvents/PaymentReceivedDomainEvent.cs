using OnePlatform.Clearing.Domain.Enums;

namespace OnePlatform.Clearing.Domain.DomainEvents;

public sealed record PaymentReceivedDomainEvent(
  Guid paymentId,
  PaymentEventType eventType,
  DateTimeOffset receivedAt,
  string? details) : IDomainEvent
{
  
}