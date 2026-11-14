using OnePlatform.Clearing.Domain.Enums;
using OnePlatform.Clearing.Domain.ValueObjects;

namespace OnePlatform.Clearing.Domain.DomainEvents;

public sealed record PaymentValidationFailedDomainEvent(
  PaymentId paymentId,
  DateTimeOffset failedAt
) : IDomainEvent;