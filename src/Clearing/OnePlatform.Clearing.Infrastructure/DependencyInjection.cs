using Microsoft.Extensions.DependencyInjection;
// using OnePlatform.Clearing.Application.Payments;
// using OnePlatform.Clearing.Application.Institutions;
// using OnePlatform.Clearing.Infrastructure.Persistence.InMemory;
using OnePlatform.Clearing.Infrastructure.Persistence.SqlServer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace OnePlatform.Clearing.Infrastructure;

public static class DependencyInjection
{
  public static IServiceCollection AddClearingInfrastructure(
    this IServiceCollection services,
    IConfiguration configuration
  )
  {
    services.AddDbContext<ClearingDbContext>(options =>
      options.UseSqlServer(configuration.GetConnectionString("Clearing")));
    // services.AddSingleton<IPaymentRepository, InMemoryPaymentRepository>();
    // services.AddSingleton<IInstitutionRepository, InMemoryInstitutionRepository>();
    services.AddSingleton(TimeProvider.System);
    return services;
  }
}