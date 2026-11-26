using MediatR;
using OnePlatform.Clearing.Shared.Domain.Exceptions;

namespace OnePlatform.Clearing.Shared.Application.Abstractions;

public interface ICommand : IRequest<Result>
{}

public interface ICommand<TResponse> : IRequest<Result<TResponse>>
{}