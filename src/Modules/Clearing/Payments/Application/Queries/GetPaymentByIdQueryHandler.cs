
using OnePlatform.Clearing.Payments.Application;
using OnePlatform.Clearing.Payments.Application.Queries;
using OnePlatform.Clearing.Payments.Domain.ValueObjects;
using OnePlatform.Clearing.Shared.Application.Abstractions;
using OnePlatform.Clearing.Shared.Domain.Exceptions;

namespace OnePlatform.Clearing.Payments.Application.Queries;

public sealed class GetPaymentByIdQueryHandler
  : IQueryHandler<GetPaymentByIdQuery, PaymentResponse>
{
  private readonly IPaymentRepository _payments;

  public GetPaymentByIdQueryHandler(IPaymentRepository paymentRepository)
  {
    _payments = paymentRepository;
  }
  public async Task<Result<PaymentResponse>> Handle(
    GetPaymentByIdQuery request,
    CancellationToken ct
  )
  {
    var idResult = PaymentId.Create(request.Id);
    if (idResult.IsFailure)
      return Result.Failure<PaymentResponse>(idResult.Error);

    var payment = await _payments.GetByIdAsync(idResult.Value, ct);

    if (payment is null)
      return Result.Failure<PaymentResponse>(
        new Error(
          "Payment.NotFound",
          $"The payment with EndToEndId {request.Id} was not found"
        )
      );

    var response = new PaymentResponse(
      payment.EndToEndId,
      payment.Amount.Amount,
      payment.Amount.Currency,
      payment.Status,
      payment.InitiatedAt
    );

    return Result.Success(response);
  }
}