using OnePlatform.Clearing.Application.Abstractions;
using OnePlatform.Clearing.Domain.Enums;
using OnePlatform.Clearing.Domain.ValueObjects;

namespace OnePlatform.Clearing.Application.Payments.Queries;

public sealed record GetPaymentByE2EQuery(string E2EId) : IQuery<PaymentResponse>;

public sealed record PaymentResponse(
  EndToEndId EndToEndId,
  decimal Amount,
  string Currency,
  PaymentStatus Status,
  DateTimeOffset InitiatedAt
);