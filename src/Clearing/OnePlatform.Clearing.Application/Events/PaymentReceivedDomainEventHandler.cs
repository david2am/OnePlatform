using MediatR;
using OnePlatform.Clearing.Domain.DomainEvents;

namespace OnePlatform.Clearing.Application.Events;

internal sealed class PaymentReceivedDomainEventHandler
  : INotificationHandler<PaymentReceivedDomainEvent>
{
  public Task Handle(
    PaymentReceivedDomainEvent notification,
    CancellationToken ct)
  {
    throw new NotImplementedException();
  }
}