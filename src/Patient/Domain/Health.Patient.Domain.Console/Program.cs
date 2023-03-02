using Health.Patient.Domain.Console.Configuration;
using Health.Patient.Domain.Console.Core;
using Health.Patient.Domain.Storage.Sql.Configuration;
using Health.Shared.Application.Broker.Configuration;
using Health.Shared.Domain.Storage.Configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using IHost = Microsoft.Extensions.Hosting.IHost;

namespace Health.Patient.Domain.Console;

public static class Program
{
    static async Task Main(string[] args)
    {
        using IHost host = CreateHostBuilder(args).Build();

        // Application code should start here.

        await host.RunAsync();
    }

    public static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .ConfigureHostConfiguration(configHost =>
            {
                var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

                configHost.SetBasePath(Directory.GetCurrentDirectory());
                configHost.AddJsonFile("appsettings.json", optional: false);
                configHost.AddJsonFile($"appsettings.{environment}.json", optional: true);
                configHost.AddEnvironmentVariables();
            }).ConfigureServices((builder, services) =>
            {
                var storageSettings = builder.Configuration
                    .GetSection("PatientDomain:PatientStorage:PatientDatabase")
                    .Get<SqlDatabaseConfigurationDto>();
                
                var brokerSettings = builder.Configuration
                    .GetSection("PatientDomain:BrokerCredentials")
                    .Get<BrokerCredentialsConfigurationDto>();
                
                services.AddDomainServices(new PatientDomainConfigurationDto(new PatientStorageConfigurationDto(storageSettings), brokerSettings));
            });
}