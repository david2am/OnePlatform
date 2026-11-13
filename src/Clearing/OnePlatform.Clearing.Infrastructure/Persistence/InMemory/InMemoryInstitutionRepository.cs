using System.Collections.Concurrent;
using OnePlatform.Clearing.Application.Institutions;
using OnePlatform.Clearing.Domain.Entities;
using OnePlatform.Clearing.Domain.ValueObjects;

namespace OnePlatform.Clearing.Infrastructure.Persistence.InMemory;

internal class InMemoryInstitutionRepository : InMemoryRepository<Institution>, IInstitutionRepository
{
  private readonly ConcurrentDictionary<Guid, Institution> _store = new();

  public Task AddAsync(Institution institution, CancellationToken ct)
  {
    Save(institution);
    return Task.CompletedTask;
  }

  public Task<Institution?> GetByRoutingNumberAsync(
    RoutingNumber routingNumber,
    CancellationToken cancellationToken
  ) =>
    Task.FromResult(Find(i => i.RoutingNumber == routingNumber));

  public Task<IReadOnlyList<Institution>> GetAllAsync(CancellationToken ct) =>
    Task.FromResult<IReadOnlyList<Institution>>(All().ToList());
}