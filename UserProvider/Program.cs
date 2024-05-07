using Azure.Messaging.ServiceBus;
using Microsoft.AspNetCore.Identity;
using Microsoft.Azure.Functions.Worker;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Diagnostics;
using UserProvider.Data.Contexts;
using UserProvider.Data.Entities;
using UserProvider.Services;

var host = new HostBuilder()
    .ConfigureFunctionsWebApplication()

   
    .ConfigureServices(services =>
    {
        services.AddApplicationInsightsTelemetryWorkerService();
        services.ConfigureFunctionsApplicationInsights();

       
        services.AddDbContext<UserDataContext>(x => x.UseSqlServer(Environment.GetEnvironmentVariable("AzureDb")));
        services.AddIdentity<UserEntity, IdentityRole>(options =>
        {
            options.Password.RequiredLength = 8;
            options.User.RequireUniqueEmail = true;
            options.SignIn.RequireConfirmedEmail = true;
        }).AddEntityFrameworkStores<UserDataContext>().AddDefaultTokenProviders();

        services.AddScoped<UserService>();

        services.AddSingleton(new ServiceBusClient(Environment.GetEnvironmentVariable("ServiceBusConnection")));


    })
    .Build();


using (var scope = host.Services.CreateScope())
{
    try
    {
        var context = scope.ServiceProvider.GetRequiredService<UserDataContext>();
        var migration = context.Database.GetPendingMigrations();
        if (migration != null && migration.Any())
        {
            context.Database.Migrate();
        }
    }
    catch (Exception ex)
    {
        Debug.WriteLine($" ERROR : UserProvider.Program.cs.Migration :: {ex.Message}");
    }
}

using (var scope = host.Services.CreateScope())
{
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
    string[] roles = ["Admin", "User"];
    foreach (var role in roles)
    {
        if (!await roleManager.RoleExistsAsync(role))
        {
            await roleManager.CreateAsync(new IdentityRole(role));
        }
    }
}


host.Run();
