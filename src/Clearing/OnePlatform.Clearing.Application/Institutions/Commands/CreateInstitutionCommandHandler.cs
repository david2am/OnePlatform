using MediatR;
using OnePlatform.Clearing.Application.Abstractions;
using OnePlatform.Clearing.Domain.Entities;
using OnePlatform.Clearing.Domain.Shared;

namespace OnePlatform.Clearing.Application.Institutions.Commands;

internal sealed class CreateInstitutionCommandHandler : ICommandHandler<CreateInstitutionCommand, Institution>
{
  private readonly IInstitutionRepository _institutions;
  private readonly TimeProvider _timeProvider;

  public CreateInstitutionCommandHandler(
    IInstitutionRepository institutions,
    TimeProvider timeProvider
  )
  {
   _institutions = institutions;
   _timeProvider = timeProvider;
  }
  public async Task<Result<Institution>> Handle(
    CreateInstitutionCommand command,
    CancellationToken cancellationToken
  )
  {
    var result = Institution.Create(
      command.Name,
      command.RoutingNumber,
      command.IsMember,
      command.MembershipSince
    );

    if (result.IsFailure)
      return Result.Failure<Institution>(result.Error);

    var institution = result.Value;

    await _institutions.AddAsync(institution, cancellationToken);

    return Result.Success(institution);
  }
}
