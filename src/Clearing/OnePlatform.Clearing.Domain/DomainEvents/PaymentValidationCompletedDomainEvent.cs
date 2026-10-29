using OnePlatform.Clearing.Domain.Enums;

namespace OnePlatform.Clearing.Domain.DomainEvents;

public sealed record PaymentValidationCompletedDomainEvent(
  Guid paymentId,
  DateTimeOffset completedAt) : IDomainEvent
{
  
}