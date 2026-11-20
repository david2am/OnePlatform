using System.Collections.Concurrent;
using Microsoft.EntityFrameworkCore;
using OnePlatform.Clearing.Application.Payments;
using OnePlatform.Clearing.Domain.Entities;
using OnePlatform.Clearing.Domain.Shared;
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
    try
    {
        await _context.Payments.AddAsync(payment, ct);
        await _context.SaveChangesAsync(ct);
    }
    catch (DbUpdateConcurrencyException)
    {
        // TODO: improve later
        // return Result.Failure(PaymentErrors.ConcurrencyConflict);
    }
  }

  public async Task<Payment?> GetByIdAsync(
    PaymentId id,
    CancellationToken ct
  ) =>
    await _context.Payments
        .Include(p => p.ValidationResults)
        .Include(p => p.StatusEvents)
        .FirstOrDefaultAsync(p => p.Id == id, ct);
        // TODO: add this validation later: && p.InstitutionId == callerInstitutionId
}