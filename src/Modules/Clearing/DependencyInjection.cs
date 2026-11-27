using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OnePlatform.Clearing.Accounts.Application;
using OnePlatform.Clearing.Accounts.Infrastructure.Persistence.SqlServer;
using OnePlatform.Clearing.Institutions.Application;
using OnePlatform.Clearing.Institutions.Infrastructure.Persistence.SqlServer;
using OnePlatform.Clearing.Payments.Application;
using OnePlatform.Clearing.Payments.Infrastructure.Persistence.SqlServer;
using OnePlatform.Clearing.Shared.Infrastructure.Persistence.SqlServer;

namespace OnePlatform.Clearing;

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

    services.AddScoped<IAccountRepository, AccountRepository>();
    services.AddScoped<IInstitutionRepository, InstitutionRepository>();
    services.AddScoped<IPaymentRepository, PaymentRepository>();

    return services;
  }

  public static IServiceCollection AddClearingApplication(
    this IServiceCollection services
  )
  {
    services.AddMediatR(cfg =>
      cfg.RegisterServicesFromAssembly(typeof(AssemblyReference).Assembly)
    );
    return services;
  }
}