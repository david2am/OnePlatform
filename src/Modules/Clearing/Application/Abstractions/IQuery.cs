using MediatR;
using OnePlatform.Clearing.Domain.Shared;

namespace OnePlatform.Clearing.Application.Abstractions;

public interface IQuery<TResponse> : IRequest<Result<TResponse>>
{
  
}