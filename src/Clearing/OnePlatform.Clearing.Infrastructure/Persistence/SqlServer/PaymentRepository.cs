using System.Collections.Concurrent;
using Microsoft.EntityFrameworkCore;
using OnePlatform.Clearing.Application.Payments;
using OnePlatform.Clearing.Domain.Entities;
using OnePlatform.Clearing.Domain.ValueObjects;

namespace OnePlatform.Clearing.Infrastructure.Persistence.SqlServer;

internal class PaymentRepository : IPaymentRepository
{
  private readonly ClearingDbContext _context;

  public PaymentRepository(ClearingDbContext context)
  {
    _context = context;
  }

  public async Task AddAsync(Payment payment, CancellationToken ct)
  {
    await _context.Payments.AddAsync(payment, ct);
    await _context.SaveChangesAsync(ct);
  }

  public async Task<Payment?> GetByE2EIdAsync(
    EndToEndId e2eId,
    CancellationToken ct
  ) =>
    await _context.Payments
        .Include(p => p.ValidationResults)
        .Include(p => p.StatusEvents)
        .FirstOrDefaultAsync(p => p.EndToEndId == e2eId, ct);
}