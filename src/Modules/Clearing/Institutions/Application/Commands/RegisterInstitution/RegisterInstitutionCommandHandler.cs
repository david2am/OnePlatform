using OnePlatform.Clearing.Institutions.Application;
using OnePlatform.Clearing.Institutions.Application.Commands;
using OnePlatform.Clearing.Institutions.Domain.Entities;
using OnePlatform.Clearing.Shared.Application.Abstractions;
using OnePlatform.Clearing.Shared.Domain.Exceptions;

namespace OnePlatform.Clearing.Institutions.Application.Commands;

internal sealed class RegisterInstitutionCommandHandler : ICommandHandler<RegisterInstitutionCommand, Institution>
{
  private readonly IInstitutionRepository _institutions;
  private readonly TimeProvider _timeProvider;

  public RegisterInstitutionCommandHandler(
    IInstitutionRepository institutions,
    TimeProvider timeProvider
  )
  {
   _institutions = institutions;
   _timeProvider = timeProvider;
  }
  public async Task<Result<Institution>> Handle(
    RegisterInstitutionCommand command,
    CancellationToken ct
  )
  {
    DateTimeOffset? membershipSince = command.IsMember
      ? command.MembershipSince ?? _timeProvider.GetUtcNow()
      : null;

    var result = Institution.Create(
      command.Name,
      command.RoutingNumber,
      command.IsMember,
      membershipSince
    );

    if (result.IsFailure)
      return Result.Failure<Institution>(result.Error);

    var institution = result.Value;

    await _institutions.AddAsync(institution, ct);

    return Result.Success(institution);
  }
}
