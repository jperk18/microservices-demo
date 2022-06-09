using Health.Appointment.Domain.Console.Core;
using Health.Appointment.Domain.Console.Core.Configuration;
using Health.Appointment.Domain.Storage.Sql.Appointment.Core.Configuration;
using Health.Appointment.Domain.Storage.Sql.ReferenceData.Core.Configuration;
using Health.Shared.Application.Configuration;
using Health.Shared.Domain.Storage.Configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using IHost = Microsoft.Extensions.Hosting.IHost;

namespace Health.Appointment.Domain.Console;

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
                    .GetSection("AppointmentDomain:AppointmentStorage:AppointmentDatabase")
                    .Get<SqlDatabaseConfiguration>();
                
                var refStorageSettings = builder.Configuration
                    .GetSection("AppointmentDomain:AppointmentStorage:ReferenceDataDatabase")
                    .Get<SqlDatabaseConfiguration>();
                
                var brokerSettings = builder.Configuration
                    .GetSection("AppointmentDomain:BrokerCredentials")
                    .Get<BrokerCredentialsConfiguration>();
                
                services.AddDomainServices(new AppointmentDomainConfiguration(new AppointmentStorageConfiguration(storageSettings), new ReferenceDataStorageConfiguration(refStorageSettings), brokerSettings));
            });
}