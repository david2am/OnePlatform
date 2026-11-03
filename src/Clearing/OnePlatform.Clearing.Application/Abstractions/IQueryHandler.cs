using MediatR;
using OnePlatform.Clearing.Domain.Shared;

namespace OnePlatform.Clearing.Application.Abstractions;

public interface IQueryHandler<TQuery, TResponse>
  : IRequestHandler<TQuery, Result<TResponse>>
  where TQuery : IQuery<TResponse>
{
  
}