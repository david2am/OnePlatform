using OnePlatform.Clearing.Application.Abstractions;
using OnePlatform.Clearing.Domain.Entities;
using OnePlatform.Clearing.Domain.ValueObjects;

namespace OnePlatform.Clearing.Application.Payments.Commands;

public sealed record ReceivePaymentCommand(
  string MessageId,
  string EndToEndId,
  Money Amount,
  PartyDetails Debtor,
  PartyDetails Creditor,
  DateTimeOffset InitiatedAt
) : ICommand<Payment>;