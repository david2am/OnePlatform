using OnePlatform.Clearing.Application.Abstractions;
using OnePlatform.Clearing.Domain.Entities;
using OnePlatform.Clearing.Domain.ValueObjects;

namespace OnePlatform.Clearing.Application.Payments.Commands;

public sealed record ReceivePaymentCommand(
  MessageId MessageId,
  EndToEndId EndToEndId,
  Money Amount,
  InstitutionId SenderInstitutionId,
  PaymentParty Debtor,
  PaymentParty Creditor,
  IReadOnlyCollection<ParticipantInput> Participants,
  DateTimeOffset InitiatedAt
) : ICommand<Payment>;