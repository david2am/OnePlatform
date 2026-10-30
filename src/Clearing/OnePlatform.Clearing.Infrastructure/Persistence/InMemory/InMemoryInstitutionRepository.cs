using System.Collections.Concurrent;
using OnePlatform.Clearing.Application.Institutions;
using OnePlatform.Clearing.Domain.Entities;

namespace OnePlatform.Clearing.Infrastructure.Persistence.InMemory;

internal class InMemoryInstitutionRepository : InMemoryRepository<Institution>, IInstitutionRepository
{
  private readonly ConcurrentDictionary<Guid, Institution> _store = new();

  public Task AddAsync(Institution institution, CancellationToken ct)
  {
    Save(institution);
    return Task.CompletedTask;
  }
}