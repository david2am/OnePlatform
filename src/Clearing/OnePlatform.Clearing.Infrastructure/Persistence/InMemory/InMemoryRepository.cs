using System.Collections.Concurrent;
using OnePlatform.Clearing.Domain.Entities;

namespace OnePlatform.Clearing.Infrastructure.Persistence.InMemory;

internal abstract class InMemoryRepository<T> where T : Entity
{
  private readonly ConcurrentDictionary<Guid, T> _store = new();

  protected void Save(T entity) => _store[entity.Id] = entity;

  protected T? Find(Guid id) =>
    _store.TryGetValue(id, out var entity) ? entity : null;

  protected IReadOnlyCollection<T> All() => _store.Values.ToList();
}