using FluentValidation;
using Health.Nurse.Domain.Console.Configuration;
using Health.Nurse.Domain.Console.Consumer;
using Health.Nurse.Domain.Console.Validators;
using Health.Nurse.Domain.Storage.Sql;
using Health.Nurse.Domain.Storage.Sql.Databases.NurseDb;
using Health.Shared.Domain;
using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using IsolationLevel = System.Transactions.IsolationLevel;

namespace Health.Nurse.Domain.Console;

public static class DependencyInjection
{
    public static void AddDomainServices(this IServiceCollection services, NurseDomainConfiguration config)
    {
        if (config == null || config.NurseStorage == null)
            throw new ApplicationException("Configuration is needed for domain services");

        //Add Validators
        services.AddValidatorsFromAssemblyContaining<RegisterNurseValidator>();

        //Add Dependant Database services
        services.AddStorageServices(config.NurseStorage);

        services.AddSharedDomainServices();

        //Services for this application
        services.AddSingleton(config);

        services.TryAddSingleton(KebabCaseEndpointNameFormatter.Instance);
        services.AddMassTransit(cfg =>
        {
            cfg.AddEntityFrameworkOutbox<NurseDbContext>(o =>
            {
                // configure which database lock provider to use (Postgres, SqlServer, or MySql)
                o.UsePostgres();
                // enable the bus outbox
                o.UseBusOutbox();
            });
            
            cfg.AddConsumersFromNamespaceContaining<RegisterNurseConsumer>();
            cfg.UsingRabbitMq(ConfigureBus);
        });
    }


    static void ConfigureBus(IBusRegistrationContext context, IRabbitMqBusFactoryConfigurator configurator)
    {
        configurator.ReceiveEndpoint("register-nurse", (e) =>
        {
            e.UseTransaction(x =>
            {
                x.Timeout = TimeSpan.FromSeconds(90);
                x.IsolationLevel = IsolationLevel.ReadCommitted;
            });

            e.ConfigureConsumer<RegisterNurseConsumer>(context);
        });

        configurator.ConfigureEndpoints(context);
    }
}