using OnePlatform.Clearing.Accounts.Domain.Entities;
using OnePlatform.Clearing.Institutions.Domain.ValueObjects;
using OnePlatform.Clearing.Shared.Application.Abstractions;
using OnePlatform.Clearing.Shared.Domain.ValueObjects;

namespace OnePlatform.Clearing.Accounts.Application.Commands;

public sealed record OpenAccountCommand(
  InstitutionId InstitutionId,
  string AccountNumber,
  Currency Currency
) : ICommand<Account>;
