using System.Collections.Concurrent;
using OnePlatform.Clearing.Application.Payments;
using OnePlatform.Clearing.Domain.Entities;
using OnePlatform.Clearing.Domain.ValueObjects;

namespace OnePlatform.Clearing.Infrastructure.Persistence.InMemory;

internal class InMemoryPaymentRepository : InMemoryRepository<Payment>, IPaymentRepository
{
  public Task AddAsync(Payment payment, CancellationToken ct)
  {
    Save(payment);
    return Task.CompletedTask;
  }

  public Task<Payment?> GetByE2EIdAsync(EndToEndId e2eId, CancellationToken ct) =>
    Task.FromResult(Find(p => p.EndToEndId == e2eId));

  public Task<IReadOnlyList<Payment>> GetAllAsync(CancellationToken ct) =>
    Task.FromResult<IReadOnlyList<Payment>>(All().ToList());
}