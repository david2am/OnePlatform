using OnePlatform.Clearing.Domain.Entities;
using OnePlatform.Clearing.Domain.ValueObjects;

namespace OnePlatform.Clearing.Application.Payments;

public interface IPaymentRepository
{
  Task AddAsync(
    Payment payment,
    CancellationToken ct = default
  );

  Task<Payment?> GetByE2EIdAsync(
    EndToEndId e2eId,
    CancellationToken ct = default
  );
}