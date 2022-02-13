using Health.Patient.Domain.Console.Core;
using Health.Patient.Domain.Storage.Sql.Core;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using IHost = Microsoft.Extensions.Hosting.IHost;

namespace Health.Patient.Domain.Console;

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
                    .GetSection("DomainConfiguration:StorageConfiguration:PatientDatabase")
                    .Get<SqlDatabaseConfiguration>();
                
                services.AddDomainServices(new DomainConfiguration(new StorageConfiguration(storageSettings)));
            });
}