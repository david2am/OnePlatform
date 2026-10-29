using MediatR;
using OnePlatform.Clearing.Application.Abstractions;
using OnePlatform.Clearing.Domain.Entities;

namespace OnePlatform.Clearing.Application.Institutions.Commands;

public sealed record CreateInstitutionCommand(
  string Name,
  string RoutingNumber,
  bool IsMember,
  DateTimeOffset? MembershipSince
) : ICommand<Institution>;