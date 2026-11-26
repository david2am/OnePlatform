
using OnePlatform.Clearing.Payments.Domain.Entities;
using OnePlatform.Clearing.Payments.Domain.ValueObjects;

namespace OnePlatform.Clearing.Payments.Application;

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