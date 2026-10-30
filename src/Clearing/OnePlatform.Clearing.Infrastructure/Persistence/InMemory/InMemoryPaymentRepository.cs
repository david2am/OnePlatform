using System.Collections.Concurrent;
using OnePlatform.Clearing.Application.Payments;
using OnePlatform.Clearing.Domain.Entities;

namespace OnePlatform.Clearing.Infrastructure.Persistence.InMemory;

internal class InMemoryPaymentRepository : InMemoryRepository<Payment>, IPaymentRepository
{
  public Task AddAsync(Payment payment, CancellationToken ct)
  {
    Save(payment);
    return Task.CompletedTask;
  }

  // public Task<Payment?> G
}