using OnePlatform.Clearing.Application.Abstractions;
using OnePlatform.Clearing.Application.Institutions;
using OnePlatform.Clearing.Application.Institutions.Queries;
using OnePlatform.Clearing.Domain.Shared;
using OnePlatform.Clearing.Domain.ValueObjects;

namespace OnePlatform.Clearing.Application.Accounts.Queries;

public sealed class GetInstitutionByIdQueryHandler
  : IQueryHandler<GetInstitutionByIdQuery, InstitutionResponse>
{
  private readonly IInstitutionRepository _institutions;

  public GetInstitutionByIdQueryHandler(
    IInstitutionRepository institutions
  )
  {
    _institutions = institutions;
  }

  public async Task<Result<InstitutionResponse>> Handle(
    GetInstitutionByIdQuery request,
    CancellationToken ct
  )
  {
    var idResult = InstitutionId.Create(request.Id);
    if (idResult.IsFailure)
      return Result.Failure<InstitutionResponse>(idResult.Error);

    var institution = await _institutions.GetByIdAsync(
      idResult.Value, ct
    );

    if (institution is null)
      return Result.Failure<InstitutionResponse>(
        new Error(
          "Institution.NotFound",
          $"The Institution with Id {request.Id} was not found"
        )
      );

    var response = new InstitutionResponse(
      institution.Name,
      institution.RoutingNumber,
      institution.IsMember,
      institution.MembershipSince
    );
    return Result.Success(response);
  }
}