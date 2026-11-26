
using OnePlatform.Clearing.Institutions.Application.Commands;
using OnePlatform.Clearing.Shared.Domain.Exceptions;
using RoutingNumberVo = OnePlatform.Clearing.Institutions.Domain.ValueObjects.RoutingNumber;

namespace OnePlatform.Api.Institutions;

public sealed record CreateInstitutionRequest(
  string Name,
  string RoutingNumber,
  bool IsMember,
  DateTimeOffset MembershipSince
)
{
  public Result<RegisterInstitutionCommand> ToCommand()
  {
    var RoutingNumberResult = RoutingNumberVo.Create(RoutingNumber);
    if (RoutingNumberResult.IsFailure)
      return Result.Failure<RegisterInstitutionCommand>(RoutingNumberResult.Error);
    
    var command = new RegisterInstitutionCommand(
      Name,
      RoutingNumberResult.Value,
      IsMember,
      MembershipSince
    );

    return Result.Success(command);
  }
}