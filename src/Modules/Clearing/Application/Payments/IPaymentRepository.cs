using OnePlatform.Clearing.Domain.Entities;
using OnePlatform.Clearing.Domain.ValueObjects;

namespace OnePlatform.Clearing.Application.Payments;

public interface IPaymentRepository
{
  Task AddAsync(
    Payment payment,
    CancellationToken ct = default
  );

  Task<Payment?> GetByIdAsync(
    PaymentId id,
    CancellationToken ct = default
  );
}