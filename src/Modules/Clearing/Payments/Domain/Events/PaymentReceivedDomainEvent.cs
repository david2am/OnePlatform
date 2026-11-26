using OnePlatform.Clearing.Payments.Domain.Enums;
using OnePlatform.Clearing.Payments.Domain.ValueObjects;
using OnePlatform.Clearing.Shared.Domain.DomainEvents;

namespace OnePlatform.Clearing.Payments.Domain.DomainEvents;

public sealed record PaymentReceivedDomainEvent(
  PaymentId PaymentId,
  PaymentEventType EventType,
  DateTimeOffset ReceivedAt,
  string? Details
) : IDomainEvent;