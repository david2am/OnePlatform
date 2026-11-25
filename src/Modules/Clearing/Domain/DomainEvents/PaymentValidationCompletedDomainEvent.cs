using OnePlatform.Clearing.Domain.Enums;
using OnePlatform.Clearing.Domain.ValueObjects;

namespace OnePlatform.Clearing.Domain.DomainEvents;

public sealed record PaymentValidationCompletedDomainEvent(
  PaymentId paymentId,
  DateTimeOffset completedAt
) : IDomainEvent;