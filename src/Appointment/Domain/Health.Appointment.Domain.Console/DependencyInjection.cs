using FluentValidation;
using Health.Appointment.Domain.Console.Configuration;
using Health.Appointment.Domain.Console.Consumers;
using Health.Appointment.Domain.Console.Validators;
using Health.Appointment.Domain.StateMachines;
using Health.Appointment.Domain.Storage.Sql.Appointment;
using Health.Appointment.Domain.Storage.Sql.Appointment.Configuration;
using Health.Appointment.Domain.Storage.Sql.Appointment.Database;
using Health.Shared.Application.Broker.Configuration;
using Health.Shared.Domain;
using Health.Shared.Workflow.Processes.Queries;
using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using IsolationLevel = System.Transactions.IsolationLevel;

namespace Health.Appointment.Domain.Console;

public static class DependencyInjection
{
    public static void AddDomainServices(this IServiceCollection services, AppointmentDomainConfiguration config)
    {
        if (config?.BrokerCredentials == null)
            throw new ApplicationException("Configuration is needed for domain services");

        //Services for this application
        services.AddSingleton(config);
        
        services.AddSharedDomainServices();

        //Add Validators
        services.AddValidatorsFromAssemblyContaining<RequestPatientCheckInValidator>();
        
        //Add Dependant Database services
        services.AddStorageServices(config.AppointmentStorageConfiguration);

        services.AddAppointmentMassTransit(config.BrokerCredentials);
    }

    private static void AddStorageServices(this IServiceCollection services,
        AppointmentStorageConfiguration configuration)
    {
        services.AddSqlAppointmentStorageServices(configuration);
    }

    private static void AddAppointmentMassTransit(this IServiceCollection services,
        BrokerCredentialsConfiguration brokerConfig)
    {
        services.TryAddSingleton(KebabCaseEndpointNameFormatter.Instance);
        services.AddMassTransit(cfg =>
        {
            cfg.AddEntityFrameworkOutbox<AppointmentStateDbContext>(o =>
            {
                // configure which database lock provider to use (Postgres, SqlServer, or MySql)
                o.UsePostgres();
                // enable the bus outbox
                o.UseBusOutbox();
            });

            cfg.AddSagaStateMachine<AppointmentStateMachine, AppointmentState, AppointmentStateDefinition>()
                .EntityFrameworkRepository(r =>
                {
                    r.ExistingDbContext<AppointmentStateDbContext>();
                    r.UsePostgres();
                });

            cfg.AddConsumersFromNamespaceContaining(typeof(GetAllWaitingPatientsConsumer));

            cfg.UsingRabbitMq((context, configurator) =>
            {
                configurator.Host(brokerConfig.Host, "/", h =>
                {
                    h.Username(brokerConfig.Username);
                    h.Password(brokerConfig.Password);
                });

                configurator.ReceiveEndpoint("request-patient-check-in", e =>
                {
                    e.UseTransaction(x =>
                    {
                        x.Timeout = TimeSpan.FromSeconds(90);
                        x.IsolationLevel = IsolationLevel.ReadCommitted;
                    });

                    e.ConfigureConsumer<RequestPatientCheckInConsumer>(context);
                });

                configurator.ReceiveEndpoint("request-schedule-appointment", e =>
                {
                    e.UseTransaction(x =>
                    {
                        x.Timeout = TimeSpan.FromSeconds(90);
                        x.IsolationLevel = IsolationLevel.ReadCommitted;
                    });

                    e.ConfigureConsumer<RequestScheduleAppointmentConsumer>(context);
                });

                configurator.ReceiveEndpoint("request-nurse-assignment-for-appointment", e =>
                {
                    e.UseTransaction(x =>
                    {
                        x.Timeout = TimeSpan.FromSeconds(90);
                        x.IsolationLevel = IsolationLevel.ReadCommitted;
                    });

                    e.ConfigureConsumer<RequestNurseAssignmentForAppointmentConsumer>(context);
                });

                configurator.ReceiveEndpoint("appointment-state", e =>
                {
                    e.UseTransaction(x =>
                    {
                        x.Timeout = TimeSpan.FromSeconds(90);
                        x.IsolationLevel = IsolationLevel.ReadCommitted;
                    });

                    e.ConfigureSaga<AppointmentState>(context);
                });
                
                configurator.ReceiveEndpoint("get-all-waiting-patients",
                    e => { e.ConfigureConsumer<GetAllWaitingPatientsConsumer>(context); });
                
                //configurator.ConfigureEndpoints(context);
            });
            
            cfg.AddRequestClient<GetNurse>();
            cfg.AddRequestClient<GetPatient>();
        });
    }
}