using OnePlatform.Clearing.Domain.Enums;
using OnePlatform.Clearing.Domain.ValueObjects;

namespace OnePlatform.Clearing.Domain.DomainEvents;

public sealed record PaymentReceivedDomainEvent(
  PaymentId paymentId,
  PaymentEventType eventType,
  DateTimeOffset receivedAt,
  string? details
) : IDomainEvent;