using MediatR;
using OnePlatform.Clearing.Application.Abstractions;

namespace OnePlatform.Clearing.Application.Institution.Commands;

public sealed record CreateInstitutionCommand() : ICommand;