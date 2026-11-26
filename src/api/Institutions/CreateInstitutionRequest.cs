using OnePlatform.Clearing.Application.Institutions.Commands;
using OnePlatform.Clearing.Domain.Shared;
using RoutingNumberVo = OnePlatform.Clearing.Domain.ValueObjects.RoutingNumber;

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