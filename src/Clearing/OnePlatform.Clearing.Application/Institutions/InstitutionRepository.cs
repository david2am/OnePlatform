using OnePlatform.Clearing.Domain.Entities;

namespace OnePlatform.Clearing.Application.Institutions;

public interface IInstitutionRepository
{
  Task AddAsync(
    Institution institution,
    CancellationToken cancellationToken = default
  );
}