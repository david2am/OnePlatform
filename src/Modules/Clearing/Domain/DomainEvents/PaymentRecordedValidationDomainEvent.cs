using OnePlatform.Clearing.Domain.Enums;

namespace OnePlatform.Clearing.Domain.DomainEvents;

public sealed record PaymentRecordedValidationDomainEvent(
  ValidationCheckType checkType,
  ValidationOutcome outcome,
  DateTimeOffset checkedAt) : IDomainEvent
{
  
}