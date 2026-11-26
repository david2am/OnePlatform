using OnePlatform.Clearing.Institutions.Domain.ValueObjects;
using OnePlatform.Clearing.Shared.Application.Abstractions;
using OnePlatform.Clearing.Shared.Domain.Exceptions;

namespace OnePlatform.Clearing.Institutions.Application.Queries;

public sealed class GetInstitutionByRoutingNumberQueryHandler
  : IQueryHandler<GetInstitutionByRoutingNumberQuery, InstitutionResponse>
{
  private readonly IInstitutionRepository _institutions;

  public GetInstitutionByRoutingNumberQueryHandler(
    IInstitutionRepository institutions
  )
  {
    _institutions = institutions;
  }

  public async Task<Result<InstitutionResponse>> Handle(
    GetInstitutionByRoutingNumberQuery request,
    CancellationToken ct
  )
  {
    var routingNumberResult = RoutingNumber.Create(request.RoutingNumber);
    if (routingNumberResult.IsFailure)
      return Result.Failure<InstitutionResponse>(routingNumberResult.Error);

    var institution = await _institutions.GetByRoutingNumberAsync(
      routingNumberResult.Value,
      ct
    );

    if (institution is null)
      return Result.Failure<InstitutionResponse>(
        new Error(
          "Institution.NotFound",
          $"The Institution with Routing Number {request.RoutingNumber} was not found"
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