using Health.Appointment.Domain.StateMachines;
using Health.Appointment.Domain.Storage.Sql.Appointment.Core;
using Health.Appointment.Domain.Storage.Sql.Appointment.Core.Configuration;
using Health.Appointment.Domain.Storage.Sql.Appointment.Database;
using Health.Shared.Application.Broker.Configuration;
using MassTransit;
using MassTransit.EntityFrameworkCoreIntegration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Health.Appointment.Domain.Storage.UnitOfWorks;

public static class DependencyInjection
{
    public static void AddStorageServices(this IServiceCollection services, AppointmentStorageConfiguration configuration)
    {
        services.AddSqlAppointmentStorageServices(configuration);
    }

    public static void AddAppointmentMassTransit(this IServiceCollection services, BrokerCredentialsConfiguration brokerConfig, Type consumersFromNamespaceContaining)
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
                });
            
            cfg.AddConsumersFromNamespaceContaining(consumersFromNamespaceContaining);

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
    }
}