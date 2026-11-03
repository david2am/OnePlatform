using OnePlatform.Clearing.Application.Abstractions;

namespace OnePlatform.Clearing.Application.Payments.Queries;

public sealed record GetPaymentByIdQuery(Guid PaymentId) : IQuery<PaymentResponse>;

public sealed record PaymentResponse(Guid Id);