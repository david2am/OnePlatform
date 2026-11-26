using OnePlatform.Clearing.Institutions.Domain.ValueObjects;
using OnePlatform.Clearing.Shared.Application.Abstractions;

namespace OnePlatform.Clearing.Institutions.Application.Queries;

public sealed record GetInstitutionByRoutingNumberQuery
(string RoutingNumber) : IQuery<InstitutionResponse>;

public sealed record InstitutionResponse(
  string Name,
  RoutingNumber RoutingNumber,
  bool IsMember,
  DateTimeOffset? MemberShipSince
);