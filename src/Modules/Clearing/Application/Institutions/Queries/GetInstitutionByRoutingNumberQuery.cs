using OnePlatform.Clearing.Application.Abstractions;
using OnePlatform.Clearing.Domain.ValueObjects;

namespace OnePlatform.Clearing.Application.Institutions.Queries;

public sealed record GetInstitutionByRoutingNumberQuery
(string RoutingNumber) : IQuery<InstitutionResponse>;

public sealed record InstitutionResponse(
  string Name,
  RoutingNumber RoutingNumber,
  bool IsMember,
  DateTimeOffset? MemberShipSince
);