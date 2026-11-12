using MediatR;
using OnePlatform.Clearing.Application.Abstractions;
using OnePlatform.Clearing.Domain.Entities;
using OnePlatform.Clearing.Domain.ValueObjects;

namespace OnePlatform.Clearing.Application.Institutions.Commands;

public sealed record CreateInstitutionCommand(
  string Name,
  RoutingNumber RoutingNumber,
  bool IsMember,
  DateTimeOffset? MembershipSince
) : ICommand<Institution>;