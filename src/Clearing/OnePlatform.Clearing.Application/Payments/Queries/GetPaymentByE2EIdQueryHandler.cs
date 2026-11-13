using OnePlatform.Clearing.Application.Abstractions;
using OnePlatform.Clearing.Domain.Shared;
using OnePlatform.Clearing.Domain.ValueObjects;

namespace OnePlatform.Clearing.Application.Payments.Queries;

public sealed class GetPaymentByIdQueryHandler
  : IQueryHandler<GetPaymentByE2EIdQuery, PaymentResponse>
{
  private readonly IPaymentRepository _payments;

  public GetPaymentByIdQueryHandler(IPaymentRepository paymentRepository)
  {
    _payments = paymentRepository;
  }
  public async Task<Result<PaymentResponse>> Handle(
    GetPaymentByE2EIdQuery request,
    CancellationToken ct
  )
  {
    var e2eIdResult = EndToEndId.Create(request.E2EId);
    if (e2eIdResult.IsFailure)
      return Result.Failure<PaymentResponse>(e2eIdResult.Error);

    var payment = await _payments.GetByE2EIdAsync(e2eIdResult.Value, ct);

    if (payment is null)
      return Result.Failure<PaymentResponse>(
        new Error(
          "Payment.NotFound",
          $"The payment with EndToEndId {request.E2EId} was not found"
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