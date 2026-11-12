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
  public Result<CreateInstitutionCommand> ToCommand()
  {
    var RoutingNumberResult = RoutingNumberVo.Create(RoutingNumber);
    if (RoutingNumberResult.IsFailure)
      return Result.Failure<CreateInstitutionCommand>(RoutingNumberResult.Error);

    var command = new CreateInstitutionCommand(
      Name,
      RoutingNumberResult.Value,
      IsMember,
      MembershipSince
    );

    return Result.Success(command);
  }
}