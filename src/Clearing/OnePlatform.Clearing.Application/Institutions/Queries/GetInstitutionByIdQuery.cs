using OnePlatform.Clearing.Application.Abstractions;
using OnePlatform.Clearing.Domain.ValueObjects;

namespace OnePlatform.Clearing.Application.Institutions.Queries;

public sealed record GetInstitutionByIdQuery
(Guid Id) : IQuery<InstitutionResponse>;