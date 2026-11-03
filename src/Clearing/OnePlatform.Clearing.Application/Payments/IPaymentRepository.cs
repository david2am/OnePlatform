using OnePlatform.Clearing.Domain.Entities;

namespace OnePlatform.Clearing.Application.Payments;

public interface IPaymentRepository
{
  Task AddAsync(
    Payment payment,
    CancellationToken ct = default
  );

  Task<Payment?> GetByE2EAsync(
    string e2eId,
    CancellationToken ct = default
  );
}