using OnePlatform.Clearing.Institutions.Domain.Entities;
using OnePlatform.Clearing.Institutions.Domain.ValueObjects;
using OnePlatform.Clearing.Shared.Application.Abstractions;
using OnePlatform.Clearing.Shared.Domain.ValueObjects;

namespace OnePlatform.Clearing.Institutions.Application.Commands;

public sealed record RegisterInstitutionCommand(
  Name Name,
  RoutingNumber RoutingNumber,
  bool IsMember,
  DateTimeOffset? MembershipSince
) : ICommand<Institution>;