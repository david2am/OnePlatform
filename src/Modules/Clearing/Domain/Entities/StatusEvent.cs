using OnePlatform.Clearing.Domain.Enums;
using OnePlatform.Clearing.Domain.ValueObjects;

namespace OnePlatform.Clearing.Domain.Entities;

public sealed class StatusEvent : Entity<Guid>
{
  private StatusEvent() : base(Guid.Empty)
  {
    PaymentId = null!;
  }

  private StatusEvent(
    Guid id,
    PaymentId paymentId,
    PaymentEventType eventType,
    DateTimeOffset occurredAt,
    string? details
  ) : base(id)
  {
    PaymentId = paymentId;
    EventType = eventType;
    OccurredAt = occurredAt;
    Details = details;
  }

  public PaymentId PaymentId { get; private set; }
  public PaymentEventType EventType { get; private set; }
  public DateTimeOffset OccurredAt { get; private set; }
  public string? Details { get; private set; }

  internal static StatusEvent Create (
    PaymentId paymentId,
    PaymentEventType eventType,
    DateTimeOffset occurredAt,
    string? details
  ) => new (Guid.CreateVersion7(), paymentId, eventType, occurredAt, details);
}