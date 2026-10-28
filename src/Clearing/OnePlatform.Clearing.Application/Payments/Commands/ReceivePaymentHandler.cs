using OnePlatform.Clearing.Application.Abstractions;
using OnePlatform.Clearing.Domain.Entities;
using OnePlatform.Clearing.Domain.Shared;

namespace OnePlatform.Clearing.Application.Payments.Commands;

public sealed class ReceivePaymentHandler : ICommandHandler<ReceivePaymentCommand, Payment>
{
  private readonly IPaymentRepository _payments;
  private readonly TimeProvider _timeProvider;

  public ReceivePaymentHandler(
    IPaymentRepository payments,
    TimeProvider timeProvider
  )
  {
    _payments = payments;
    _timeProvider = timeProvider;
  }

  public async Task<Result<Payment>> Handle(
    ReceivePaymentCommand command,
    CancellationToken cancellationToken = default
  )
  {
    var receivedAt = _timeProvider.GetUtcNow();

    var result = Payment.Receive(
      command.MessageId,
      command.EndToEndId,
      command.Amount,
      command.Debtor,
      command.Creditor,
      command.InitiatedAt,
      receivedAt
    );

    if (result.IsFailure)
      return Result.Failure<Payment>(result.Error);

    var payment = result.Value;

    await _payments.AddAsync(payment, cancellationToken);

    return Result.Success(payment);
  }
}