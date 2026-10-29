using OnePlatform.Clearing.Domain.Entities;

namespace OnePlatform.Clearing.Application.Payments;

public interface IPaymentRepository
{
  Task AddAsync(
    Payment payment,
    CancellationToken cancellationToken = default
  );
}