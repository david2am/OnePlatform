
using InstitutionIdVo = OnePlatform.Clearing.Institutions.Domain.ValueObjects.InstitutionId;
using CurrencyVo = OnePlatform.Clearing.Shared.Domain.ValueObjects.Currency;
using OnePlatform.Clearing.Shared.Domain.Exceptions;
using OnePlatform.Clearing.Accounts.Application.Commands;

namespace OnePlatform.Api.Accounts;

public sealed record OpenAccountRequest(
  Guid InstitutionId,
  string AccountNumber,
  string Currency
)
{
  public Result<OpenAccountCommand> ToCommand()
  {
    var institutionIdResult = InstitutionIdVo.Create(InstitutionId);
    if (institutionIdResult.IsFailure)
      return Result.Failure<OpenAccountCommand>(institutionIdResult.Error);
    
    var currencyResult = CurrencyVo.Create(Currency);
    if (currencyResult.IsFailure)
      return Result.Failure<OpenAccountCommand>(currencyResult.Error);

    var command = new OpenAccountCommand(
      institutionIdResult.Value,
      AccountNumber,
      currencyResult.Value
    );
    return Result.Success(command);
  }
}