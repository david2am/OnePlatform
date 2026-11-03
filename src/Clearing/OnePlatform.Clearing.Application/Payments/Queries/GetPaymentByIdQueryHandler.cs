using OnePlatform.Clearing.Application.Abstractions;
using OnePlatform.Clearing.Domain.Shared;

namespace OnePlatform.Clearing.Application.Payments.Queries;

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
    var payment = await _payments.GetByIdAsync(request.PaymentId, ct);

    if (payment is null)
      return Result.Failure<PaymentResponse>(
        new Error(
          "Payment.NotFound",
          $"The payment with Id {request.PaymentId} was not found"
        )
      );

    var response = new PaymentResponse(payment.Id);

    return Result.Success(response);
  }
}