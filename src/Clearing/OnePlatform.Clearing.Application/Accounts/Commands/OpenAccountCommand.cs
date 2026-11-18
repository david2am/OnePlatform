using OnePlatform.Clearing.Application.Abstractions;
using OnePlatform.Clearing.Domain.Entities;
using OnePlatform.Clearing.Domain.ValueObjects;

namespace OnePlatform.Clearing.Application.Accounts.Commands;

public sealed record OpenAccountCommand(
  InstitutionId InstitutionId,
  string AccountNumber,
  string Currency
) : ICommand<Account>;
