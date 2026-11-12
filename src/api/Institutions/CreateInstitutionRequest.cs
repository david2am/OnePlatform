using OnePlatform.Clearing.Application.Institutions.Commands;
using OnePlatform.Clearing.Domain.Shared;
using OnePlatform.Clearing.Domain.ValueObjects;

namespace OnePlatform.Api.Institutions;

public sealed record CreateInstitutionRequest(
  string Name,
  string InstitutionRoutingNumber,
  bool IsMember,
  DateTimeOffset MembershipSince
)
{
  public Result<CreateInstitutionCommand> ToCommand()
  {
    var RoutingNumberResult = RoutingNumber.Create(InstitutionRoutingNumber);
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