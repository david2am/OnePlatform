using OnePlatform.Clearing.Institutions.Domain.ValueObjects;
using OnePlatform.Clearing.Shared.Application.Abstractions;
using OnePlatform.Clearing.Shared.Domain.ValueObjects;

namespace OnePlatform.Clearing.Institutions.Application.Queries;

public sealed record GetInstitutionByRoutingNumberQuery
(string RoutingNumber) : IQuery<InstitutionResponse>;

public sealed record InstitutionResponse(
  Name Name,
  RoutingNumber RoutingNumber,
  bool IsMember,
  DateTimeOffset? MemberShipSince
);