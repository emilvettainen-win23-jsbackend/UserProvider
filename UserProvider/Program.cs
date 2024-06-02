using Microsoft.Azure.Functions.Worker;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using UserProvider.Configurations;
using UserProvider.Infrastructure.Data.Contexts;


var host = new HostBuilder()
    .ConfigureFunctionsWebApplication()

   
    .ConfigureServices((context, services) =>
    {
        services.AddApplicationInsightsTelemetryWorkerService();
        services.ConfigureFunctionsApplicationInsights();

        

        services.AddDbContextFactory<UserDataContext>(options => options.UseSqlServer(context.Configuration.GetValue<string>("AzureDb")));

        services.RegisterServices();
        services.RegisterRepositories();
     

      


    })
    .Build();





host.Run();
