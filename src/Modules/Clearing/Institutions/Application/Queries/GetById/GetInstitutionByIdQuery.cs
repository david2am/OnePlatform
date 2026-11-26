using OnePlatform.Clearing.Shared.Application.Abstractions;

namespace OnePlatform.Clearing.Institutions.Application.Queries;

public sealed record GetInstitutionByIdQuery
(Guid Id) : IQuery<InstitutionResponse>;