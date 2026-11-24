using OnePlatform.Clearing.Application.Abstractions;

namespace OnePlatform.Clearing.Application.Accounts.Queries;

public sealed record GetAccountsByIdQuery(
  Guid Id
) : IQuery<AccountResponse>;