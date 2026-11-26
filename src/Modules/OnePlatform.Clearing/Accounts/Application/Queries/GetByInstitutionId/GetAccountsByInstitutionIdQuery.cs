using OnePlatform.Clearing.Accounts.Domain.Enums;
using OnePlatform.Clearing.Accounts.Domain.ValueObjects;
using OnePlatform.Clearing.Institutions.Domain.ValueObjects;
using OnePlatform.Clearing.Shared.Application.Abstractions;
using OnePlatform.Clearing.Shared.Domain.ValueObjects;

namespace OnePlatform.Clearing.Accounts.Application.Queries;

public sealed record GetAccountsByInstitutionIdQuery(
  Guid InstitutionId
) : IQuery<IReadOnlyList<AccountResponse>>;

public sealed record AccountResponse(
  AccountId Id,
  InstitutionId InstitutionId,
  string AccountNumber,
  Currency Currency,
  AccountStatus Status,
  DateTimeOffset OpenedAt
);