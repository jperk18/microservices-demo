using Health.Appointment.Domain.Console.Core.Configuration;
using Health.Appointment.Domain.Console.Core.Pipelines;
using Health.Appointment.Domain.StateMachines;
using Health.Appointment.Domain.Storage.Sql.Core;
using Health.Appointment.Domain.Storage.Sql.Core.Databases.AppointmentState;
using Health.Shared.Domain.Core;
using Health.Shared.Domain.Core.Configurations;
using Health.Shared.Domain.Core.RegistrationHelpers;
using MassTransit;
using MassTransit.Definition;
using MassTransit.EntityFrameworkCoreIntegration;
using MassTransit.Transactions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Health.Appointment.Domain.Console.Core;

public static class DependencyInjection
{
    public static void AddDomainServices(this IServiceCollection services, IAppointmentDomainConfiguration config)
    {
        if (config == null || config.BrokerCredentials == null)
            throw new ApplicationException("Configuration is needed for domain services");

        //Add Dependant Database services
        services.AddStorageServices(config.StorageConfiguration);
        
        //Add Core services (serialization and Transaction handling)
        var handlerTypes = typeof(Program).Assembly.GetTypes()
            .Where(x => x.GetInterfaces().Any(y => Handlers.IsHandlerInterface(y)))
            .Where(x => x.Name.EndsWith("Handler"))
            .ToList(); //This assembly Handlers

        services.AddCoreDomainServices(handlerTypes, new List<PipelineConfigurationDto>()
        {
            new(typeof(AppointmentTransactionPipelineAttribute)){
                CommandHandler = typeof(AppointmentTransactionCommandDecorator<,>),
                QueryHandler = null //Transaction pipeline not supported in queries
            }
        });

        //Services for this application
        services.AddSingleton(config);
        services.TryAddSingleton(KebabCaseEndpointNameFormatter.Instance);
        services.AddMassTransit(cfg =>
        {
            cfg.AddSagaStateMachine<AppointmentStateMachine, AppointmentState>()
                .EntityFrameworkRepository(r =>
                {
                    r.ConcurrencyMode = ConcurrencyMode.Pessimistic; // or use Optimistic, which requires RowVersion

                    r.ExistingDbContext<AppointmentStateDbContext>();
                    r.LockStatementProvider = new PostgresLockStatementProvider();
                    // r.AddDbContext<DbContext, AppointmentStateDbContext>((provider, builder) =>
                    // {
                    //     builder.UseInMemoryDatabase("Appointment");
                    //     // builder.UseSqlServer(connectionString, m =>
                    //     // {
                    //     //     m.MigrationsAssembly(Assembly.GetExecutingAssembly().GetName().Name);
                    //     //     m.MigrationsHistoryTable($"__{nameof(OrderStateDbContext)}");
                    //     // });
                    // });
                });
            
            cfg.UsingRabbitMq((context, configurator) =>
            {
                configurator.Host(config.BrokerCredentials.Host, "/", h =>
                {
                    h.Username(config.BrokerCredentials.Username);
                    h.Password(config.BrokerCredentials.Password);
                });
                configurator.ConfigureEndpoints(context);
            });
            
            cfg.AddTransactionalBus();
        });

        services.AddMassTransitHostedService();
    }
}