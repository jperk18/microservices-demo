using System.Reflection;
using Health.Workflow.Domain.Console.Db;
using Health.Workflow.Domain.Console.StateMachines;
using MassTransit;
using MassTransit.Definition;
using MassTransit.EntityFrameworkCoreIntegration;
using MassTransit.EntityFrameworkCoreIntegration.JobService;
using MassTransit.JobService.Components.StateMachines;
using MassTransit.RabbitMqTransport;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using IHost = Microsoft.Extensions.Hosting.IHost;

namespace Health.Workflow.Domain.Console;

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
            }).ConfigureServices((hostcontext, services) =>
            {
                services.TryAddSingleton(KebabCaseEndpointNameFormatter.Instance);

                services.AddMassTransit(cfg =>
                {
                    cfg.AddSagaStateMachine<AppointmentStateMachine, AppointmentState>()
                        .EntityFrameworkRepository(r =>
                        {
                            r.ConcurrencyMode = ConcurrencyMode.Pessimistic; // or use Optimistic, which requires RowVersion

                            r.AddDbContext<DbContext, AppointmentStateDbContext>((provider,builder) =>
                            {
                                builder.UseInMemoryDatabase("Appointment");
                                // builder.UseSqlServer(connectionString, m =>
                                // {
                                //     m.MigrationsAssembly(Assembly.GetExecutingAssembly().GetName().Name);
                                //     m.MigrationsHistoryTable($"__{nameof(OrderStateDbContext)}");
                                // });
                            });
                        });

                    cfg.UsingRabbitMq(ConfigureBus);
                });

                services.AddMassTransitHostedService();
            });

    static void ConfigureBus(IBusRegistrationContext context, IRabbitMqBusFactoryConfigurator configurator)
    {
        configurator.ConfigureEndpoints(context);
    }
}
