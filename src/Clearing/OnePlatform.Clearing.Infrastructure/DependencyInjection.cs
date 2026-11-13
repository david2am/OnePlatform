using Microsoft.Extensions.DependencyInjection;
using OnePlatform.Clearing.Infrastructure.Persistence.SqlServer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using OnePlatform.Clearing.Application.Payments;
using OnePlatform.Clearing.Application.Institutions;
using OnePlatform.Clearing.Infrastructure.Persistence.InMemory;

namespace OnePlatform.Clearing.Infrastructure;

public static class DependencyInjection
{
  public static IServiceCollection AddClearingInfrastructure(
    this IServiceCollection services,
    IConfiguration configuration
  )
  {
    var connectionString =
      configuration.GetConnectionString("Clearing")
      ?? throw new InvalidOperationException("Connection string 'Clearing' was not found.");

    services.AddDbContext<ClearingDbContext>
      (options =>
          options.UseSqlServer(connectionString)
      );

    services.AddSingleton<TimeProvider>(TimeProvider.System);

    services.AddScoped<IPaymentRepository, PaymentRepository>();
    services.AddScoped<IInstitutionRepository, InstitutionRepository>();

    return services;
  }
}