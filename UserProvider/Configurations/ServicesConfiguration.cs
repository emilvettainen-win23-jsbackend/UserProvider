using Microsoft.Extensions.DependencyInjection;
using UserProvider.Infrastructure.Services;

namespace UserProvider.Configurations;

public static class ServicesConfiguration
{
    public static void RegisterServices(this IServiceCollection services)
    {
        services.AddScoped<UserService>();
        services.AddHttpClient();
    }
}
