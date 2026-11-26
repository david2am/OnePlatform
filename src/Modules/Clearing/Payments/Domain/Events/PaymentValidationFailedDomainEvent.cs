using OnePlatform.Clearing.Payments.Domain.ValueObjects;
using OnePlatform.Clearing.Shared.Domain.DomainEvents;

namespace OnePlatform.Clearing.Payments.Domain.DomainEvents;

public sealed record PaymentValidationFailedDomainEvent(
  PaymentId PaymentId,
  DateTimeOffset FailedAt
) : IDomainEvent;