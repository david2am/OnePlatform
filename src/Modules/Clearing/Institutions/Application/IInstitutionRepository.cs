using OnePlatform.Clearing.Institutions.Domain.Entities;
using OnePlatform.Clearing.Institutions.Domain.ValueObjects;

namespace OnePlatform.Clearing.Institutions.Application;

public interface IInstitutionRepository
{
  Task AddAsync(
    Institution institution,
    CancellationToken ct = default
  );

  Task<Institution?> GetByIdAsync(
    InstitutionId id,
    CancellationToken ct = default
  );

  Task<Institution?> GetByRoutingNumberAsync(
    RoutingNumber routingNumber,
    CancellationToken ct = default
  );
}