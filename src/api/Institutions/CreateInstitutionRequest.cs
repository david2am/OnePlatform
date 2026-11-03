using OnePlatform.Clearing.Application.Institutions.Commands;
using OnePlatform.Clearing.Domain.Shared;

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
    var command = new CreateInstitutionCommand(
      Name,
      RoutingNumber,
      IsMember,
      MembershipSince
    );

    return Result.Success(command);
  }
}