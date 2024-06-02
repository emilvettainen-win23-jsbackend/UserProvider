using Microsoft.Extensions.DependencyInjection;
using UserProvider.Infrastructure.Data.Repositories;

namespace UserProvider.Configurations;

public static class RepositoriesConfiguration
{
    public static void RegisterRepositories(this IServiceCollection services)
    {
        services.AddScoped<UserRepository>();
    }
}