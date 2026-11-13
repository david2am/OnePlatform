using OnePlatform.Clearing.Domain.Entities;
using OnePlatform.Clearing.Domain.ValueObjects;

namespace OnePlatform.Clearing.Application.Institutions;

public interface IInstitutionRepository
{
  Task AddAsync(
    Institution institution,
    CancellationToken ct = default
  );

  Task<Institution?> GetByRoutingNumberAsync(
    RoutingNumber routingNumber,
    CancellationToken ct = default
  );
}