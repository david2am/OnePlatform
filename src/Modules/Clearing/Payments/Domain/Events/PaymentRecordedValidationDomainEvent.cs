using OnePlatform.Clearing.Payments.Domain.Enums;
using OnePlatform.Clearing.Shared.Domain.DomainEvents;

namespace OnePlatform.Clearing.Payments.Domain.DomainEvents;

public sealed record PaymentRecordedValidationDomainEvent(
  ValidationCheckType CheckType,
  ValidationOutcome Outcome,
  DateTimeOffset CheckedAt
) : IDomainEvent;