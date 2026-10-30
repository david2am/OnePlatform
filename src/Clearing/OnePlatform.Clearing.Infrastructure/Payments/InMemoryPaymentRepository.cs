using System.Collections.Concurrent;
using OnePlatform.Clearing.Application.Payments;
using OnePlatform.Clearing.Domain.Entities;

namespace OnePlatform.Clearing.Infrastructure.Payments;

public class InMemoryPaymentRepository : IPaymentRepository
{
  private readonly ConcurrentDictionary<Guid, Payment> _store = new();

  public Task AddAsync(Payment payment, CancellationToken ct)
  {
    _store[payment.Id] = payment;
    return Task.CompletedTask;
  }

  // public Task<Payment?> G
}