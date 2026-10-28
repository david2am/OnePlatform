using MediatR;
using OnePlatform.Clearing.Application.Abstractions;
using OnePlatform.Clearing.Domain.Shared;

namespace OnePlatform.Clearing.Application.Institution.Commands;

internal sealed class CreateInstitutionCommandHandler : ICommandHandler<CreateInstitutionCommand>
{
  // public CreateInstitutionCommand(

  // )
  // {
    
  // }
  public async Task<Result> Handle(CreateInstitutionCommand request, CancellationToken cancellationToken)
  {
    return Result.Success();
  }
}
