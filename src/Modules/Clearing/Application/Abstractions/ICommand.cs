using MediatR;
using OnePlatform.Clearing.Domain.Shared;

namespace OnePlatform.Clearing.Application.Abstractions;

public interface ICommand : IRequest<Result>
{
  
}

public interface ICommand<TResponse> : IRequest<Result<TResponse>>
{
  
}