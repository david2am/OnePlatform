using OnePlatform.Clearing.Application.Abstractions;
using OnePlatform.Clearing.Domain.Shared;

namespace OnePlatform.Clearing.Application.Payments.Queries;

public sealed class GetPaymentByIdQueryHandler
  : IQueryHandler<GetPaymentByE2EQuery, PaymentResponse>
{
  private readonly IPaymentRepository _payments;

  public GetPaymentByIdQueryHandler(IPaymentRepository paymentRepository)
  {
    _payments = paymentRepository;
  }
  public async Task<Result<PaymentResponse>> Handle(
    GetPaymentByE2EQuery request,
    CancellationToken ct
  )
  {
    var payment = await _payments.GetByE2EAsync(request.E2EId, ct);

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