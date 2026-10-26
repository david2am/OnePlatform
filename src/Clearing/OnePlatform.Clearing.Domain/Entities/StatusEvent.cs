using OnePlatform.Clearing.Domain.Enums;

namespace OnePlatform.Clearing.Domain.Entities;

public sealed class StatusEvent : Entity
{
  private StatusEvent() : base(Guid.Empty) {}

  private StatusEvent(
    Guid id,
    Guid paymentId,
    PaymentEventType eventType,
    DateTime occurredAt,
    string? details
  ) : base(id)
  {
    PaymentId = paymentId;
    EventType = eventType;
    OccurredAt = occurredAt;
    Details = details;
  }

  public Guid PaymentId { get; private set; }
  public PaymentEventType EventType { get; private set; }
  public DateTime OccurredAt { get; private set; }
  public string? Details { get; private set; }

  internal static StatusEvent Create (
    Guid paymentId,
    PaymentEventType eventType,
    DateTime occurredAt,
    string? details
  ) => new (Guid.NewGuid(), paymentId, eventType, occurredAt, details);
}