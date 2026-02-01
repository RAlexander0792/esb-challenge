using ESBC.Application;
using ESBC.Core;
using ESBC.Data;
using ESBC.UtilitiesCli.ApiProviders;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Refit;

namespace ESBC.UtilitiesCli;

public static class DependencyManager
{
    public static IServiceProvider ServiceProvider;

    public static void Initiate()
    {
        var serviceCollection = new ServiceCollection();

        IConfiguration configuration = new ConfigurationBuilder()
          .SetBasePath(Directory.GetCurrentDirectory())
          .AddJsonFile("appSettings.json", false)
          .Build();

        serviceCollection.AddSingleton<IConfiguration>(configuration);
        serviceCollection.AddOptions<OptionsContext>()
           .BindConfiguration("Options")
           .Validate(o => !string.IsNullOrWhiteSpace(o.DbConnectionString), "ConnectionString is required")
           .Validate(o => !string.IsNullOrWhiteSpace(o.DatabaseName), "DatabaseName is required")
           .ValidateOnStart();

        serviceCollection.AddDatabase();
        serviceCollection.AddApplicationServices();
        
        serviceCollection.AddRefitClient<IPlayerProvider>()
            .ConfigureHttpClient(c => c.BaseAddress = new Uri("https://localhost:7010/api/v1"));

        serviceCollection.AddRefitClient<IMatchProvider>()
            .ConfigureHttpClient(c => c.BaseAddress = new Uri("https://localhost:7010/api/v1"));



        ServiceProvider = serviceCollection.BuildServiceProvider();
    }
}
