using OnePlatform.Clearing.Institutions.Domain.Entities;
using OnePlatform.Clearing.Institutions.Domain.ValueObjects;
using OnePlatform.Clearing.Shared.Application.Abstractions;

namespace OnePlatform.Clearing.Institutions.Application.Commands;

public sealed record RegisterInstitutionCommand(
  string Name,
  RoutingNumber RoutingNumber,
  bool IsMember,
  DateTimeOffset? MembershipSince
) : ICommand<Institution>;