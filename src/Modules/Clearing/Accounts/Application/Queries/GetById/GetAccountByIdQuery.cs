using OnePlatform.Clearing.Shared.Application.Abstractions;

namespace OnePlatform.Clearing.Accounts.Application.Queries;

public sealed record GetAccountsByIdQuery(
  Guid Id
) : IQuery<AccountResponse>;