
using OnePlatform.Clearing.Payments.Domain.Enums;
using OnePlatform.Clearing.Payments.Domain.ValueObjects;
using OnePlatform.Clearing.Shared.Application.Abstractions;
using OnePlatform.Clearing.Shared.Domain.ValueObjects;

namespace OnePlatform.Clearing.Payments.Application.Queries;

public sealed record GetPaymentByIdQuery(Guid Id) : IQuery<PaymentResponse>;

public sealed record PaymentResponse(
  EndToEndId EndToEndId,
  decimal Amount,
  Currency Currency,
  PaymentStatus Status,
  DateTimeOffset InitiatedAt
);