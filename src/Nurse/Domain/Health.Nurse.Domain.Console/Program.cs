using Health.Nurse.Domain.Console.Core;
using Health.Nurse.Domain.Console.Core.Configuration;
using Health.Nurse.Domain.Storage.Sql.Core;
using Health.Nurse.Domain.Storage.Sql.Core.Configuration.Inner;
using Health.Shared.Application.Broker.Configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using IHost = Microsoft.Extensions.Hosting.IHost;

namespace Health.Nurse.Domain.Console;

static class Program
{
    static async Task Main(string[] args)
    {
        using IHost host = CreateHostBuilder(args).Build();

        // Application code should start here.

        await host.RunAsync();
    }

    static IHostBuilder CreateHostBuilder(string[] args) =>
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
                    .GetSection("NurseDomain:NurseStorage:NurseDatabase")
                    .Get<SqlDatabaseConfiguration>();
                
                var brokerSettings = builder.Configuration
                    .GetSection("NurseDomain:BrokerCredentials")
                    .Get<BrokerCredentialsConfiguration>();
                
                services.AddDomainServices(new NurseDomainConfiguration(new NurseStorageConfiguration(storageSettings), brokerSettings));
            });
}