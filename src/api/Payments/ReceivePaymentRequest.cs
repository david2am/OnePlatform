using OnePlatform.Clearing.Application.Payments.Commands;
using OnePlatform.Clearing.Domain.Shared;
using OnePlatform.Clearing.Domain.ValueObjects;

namespace OnePlatform.Api.Payments;

public sealed record ReceivePaymentRequest(
  string MessageId,
  string EndToEndId,
  // Money
  long Amount,
  string Currency,
  // Debtor
  string DebtorAccountNumber,
  string DebtorName,
  Guid DebtorInstitutionId,
  // Creditor
  string CreditorAccountNumber,
  string CreditorName,
  Guid CreditorInstitutionId,

  DateTimeOffset InitiatedAt
)
{
  public Result<ReceivePaymentCommand> ToCommand()
  {
    var amountResult = Money.Create(Amount, Currency);
    if (amountResult.IsFailure)
      return Result.Failure<ReceivePaymentCommand>(amountResult.Error);

    var debtorResult = PartyDetails.Create(
      DebtorAccountNumber,
      DebtorName,
      DebtorInstitutionId
    );
    if (debtorResult.IsFailure)
      return Result.Failure<ReceivePaymentCommand>(debtorResult.Error);

    var creditorResult = PartyDetails.Create(
      CreditorAccountNumber,
      CreditorName,
      CreditorInstitutionId
    );
    if (creditorResult.IsFailure)
      return Result.Failure<ReceivePaymentCommand>(creditorResult.Error);
    
    var command = new ReceivePaymentCommand(
        MessageId,
        EndToEndId,
        amountResult.Value,
        debtorResult.Value,
        creditorResult.Value,
        InitiatedAt
    );

    return Result.Success(command);
  }
}