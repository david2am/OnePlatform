using OnePlatform.Clearing.Institutions.Domain.ValueObjects;
using OnePlatform.Clearing.Payments.Domain.Entities;
using OnePlatform.Clearing.Payments.Domain.ValueObjects;
using OnePlatform.Clearing.Shared.Application.Abstractions;
using OnePlatform.Clearing.Shared.Domain.ValueObjects;

namespace OnePlatform.Clearing.Payments.Application.Commands;

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