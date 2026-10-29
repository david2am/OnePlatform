using Microsoft.Extensions.DependencyInjection;
using OnePlatform.Clearing.Application.Payments;

namespace OnePlatform.Clearing.Infrastructure;

public static class DependencyInjection
{
  public static IServiceCollection AddClearingInfrastructure(this IServiceCollection services)
  {
    // services.AddSingleton<IPaymentRepository, InMemoryPaymentRepository>(); TODO: uncomment later
    services.AddSingleton(TimeProvider.System);
    return services;
  }
}