using Microsoft.Extensions.DependencyInjection;
using OnePlatform.Clearing.Application.Payments;
using OnePlatform.Clearing.Application.Institutions;
using OnePlatform.Clearing.Infrastructure.Persistence.InMemory;

namespace OnePlatform.Clearing.Infrastructure;

public static class DependencyInjection
{
  public static IServiceCollection AddClearingInfrastructure(this IServiceCollection services)
  {
    services.AddSingleton<IPaymentRepository, InMemoryPaymentRepository>();
    services.AddSingleton<IInstitutionRepository, InMemoryInstitutionRepository>();
    services.AddSingleton(TimeProvider.System);
    return services;
  }
}