using System.Collections.Concurrent;
using OnePlatform.Clearing.Application.Institutions;
using OnePlatform.Clearing.Domain.Entities;

namespace OnePlatform.Clearing.Infrastructure.Institutions;

public class InMemoryInstitutionRepository : IInstitutionRepository
{
  private readonly ConcurrentDictionary<Guid, Institution> _store = new();

  public Task AddAsync(Institution institution, CancellationToken ct)
  {
    _store[institution.Id] = institution;
    return Task.CompletedTask;
  }
}