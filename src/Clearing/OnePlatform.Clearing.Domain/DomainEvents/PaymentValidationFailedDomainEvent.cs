using OnePlatform.Clearing.Domain.Enums;

namespace OnePlatform.Clearing.Domain.DomainEvents;

public sealed record PaymentValidationFailedDomainEvent(
  Guid paymentId,
  DateTimeOffset failedAt) : IDomainEvent
{
  
}