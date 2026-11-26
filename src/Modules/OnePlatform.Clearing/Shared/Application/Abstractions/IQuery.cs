using MediatR;
using OnePlatform.Clearing.Shared.Domain.Exceptions;

namespace OnePlatform.Clearing.Shared.Application.Abstractions;

public interface IQuery<TResponse> : IRequest<Result<TResponse>>
{}