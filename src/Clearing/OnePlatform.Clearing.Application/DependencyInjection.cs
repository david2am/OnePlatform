using Microsoft.Extensions.DependencyInjection;

namespace OnePlatform.Clearing.Application;

public static class DependencyInjection
{
  public static IServiceCollection AddClearingApplication(this IServiceCollection services)
  {
    services.AddMediatR(cfg =>
      cfg.RegisterServicesFromAssembly(typeof(AssemblyReference).Assembly)
    );
    return services;
  }
}