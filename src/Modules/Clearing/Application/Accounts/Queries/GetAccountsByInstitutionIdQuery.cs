using OnePlatform.Clearing.Application.Abstractions;
using OnePlatform.Clearing.Domain.Enums;
using OnePlatform.Clearing.Domain.ValueObjects;

namespace OnePlatform.Clearing.Application.Accounts.Queries;

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