using MediatR;
using OnePlatform.Clearing.Shared.Domain.Exceptions;

namespace OnePlatform.Clearing.Shared.Application.Abstractions;

public interface IQueryHandler<TQuery, TResponse>
  : IRequestHandler<TQuery, Result<TResponse>>
  where TQuery : IQuery<TResponse>
{}