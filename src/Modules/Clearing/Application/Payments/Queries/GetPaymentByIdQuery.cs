using OnePlatform.Clearing.Application.Abstractions;
using OnePlatform.Clearing.Domain.Enums;
using OnePlatform.Clearing.Domain.ValueObjects;

namespace OnePlatform.Clearing.Application.Payments.Queries;

public sealed record GetPaymentByIdQuery(Guid Id) : IQuery<PaymentResponse>;

public sealed record PaymentResponse(
  EndToEndId EndToEndId,
  decimal Amount,
  Currency Currency,
  PaymentStatus Status,
  DateTimeOffset InitiatedAt
);