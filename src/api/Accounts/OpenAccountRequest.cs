using OnePlatform.Clearing.Application.Accounts.Commands;
using OnePlatform.Clearing.Domain.Shared;
using InstitutionIdVo = OnePlatform.Clearing.Domain.ValueObjects.InstitutionId;

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

    var command = new OpenAccountCommand(
      institutionIdResult.Value,
      AccountNumber,
      Currency
    );
    return Result.Success(command);
  }
}