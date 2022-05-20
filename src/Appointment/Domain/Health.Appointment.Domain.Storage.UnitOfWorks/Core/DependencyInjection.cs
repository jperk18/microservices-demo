using Health.Appointment.Domain.StateMachines;
using Health.Appointment.Domain.Storage.Sql.Appointment.Core;
using Health.Appointment.Domain.Storage.Sql.Appointment.Core.Configuration;
using Health.Appointment.Domain.Storage.Sql.Appointment.Database;
using Health.Appointment.Domain.Storage.Sql.ReferenceData.Core;
using Health.Appointment.Domain.Storage.Sql.ReferenceData.Core.Configuration;
using Health.Shared.Application.Configuration;
using Health.Shared.Workflow.Processes.Queries;
using MassTransit;
using MassTransit.Definition;
using MassTransit.EntityFrameworkCoreIntegration;
using MassTransit.Transactions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Health.Appointment.Domain.Storage.UnitOfWorks.Core;

public static class DependencyInjection
{
    public static void AddUnitsOfWork(this IServiceCollection services, IAppointmentStorageConfiguration configuration, IReferenceDataStorageConfiguration config)
    {
        services.AddStorageServices(configuration);
        services.AddReferenceDataStorageServices(config);
        services.AddTransient<IAppointmentUnitOfWork, AppointmentUnitOfWork>();
    }
    
    public static void AddAppointmentMassTransit(this IServiceCollection services, IBrokerCredentialsConfiguration brokerConfig)
    {
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
            
            cfg.AddConsumersFromNamespaceContaining<GetAllWaitingPatients>();

            cfg.UsingRabbitMq((context, configurator) =>
            {
                configurator.Host(brokerConfig.Host, "/", h =>
                {
                    h.Username(brokerConfig.Username);
                    h.Password(brokerConfig.Password);
                });
                configurator.ConfigureEndpoints(context);
            });
            
            cfg.AddTransactionalBus();
        });

        services.AddMassTransitHostedService();
    }
}