using System.Transactions;
using FluentValidation.AspNetCore;
using Health.Patient.Domain.Console.Configuration;
using Health.Patient.Domain.Console.Consumer;
using Health.Patient.Domain.Console.Services;
using Health.Patient.Domain.Console.Validators;
using Health.Patient.Domain.Storage.Sql;
using Health.Patient.Domain.Storage.Sql.Databases.PatientDb;
using Health.Shared.Domain;
using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Health.Patient.Domain.Console;

public static class DependencyInjection
{
    public static void AddDomainServices(this IServiceCollection services, PatientDomainConfiguration config)
    {
        if (config == null || config.PatientStorage == null)
            throw new ApplicationException("Configuration is needed for domain services");

        services.AddFluentValidation(fv =>
            fv.RegisterValidatorsFromAssemblyContaining<RegisterPatientValidator>());

        //Add Dependant Database services
        services.AddStorageServices(config.PatientStorage);
        services.AddSharedDomainServices();
        
        //Services for this application
        services.AddSingleton(config);
        services.AddScoped(typeof(IPatientValidationService<>), typeof(PatientValidationService<>));

        services.TryAddSingleton(KebabCaseEndpointNameFormatter.Instance);
        services.AddMassTransit(cfg =>
        {
            cfg.AddEntityFrameworkOutbox<PatientDbContext>(o =>
            {
                // configure which database lock provider to use (Postgres, SqlServer, or MySql)
                o.UsePostgres();
                // enable the bus outbox
                o.UseBusOutbox();
            });
            
            cfg.AddConsumersFromNamespaceContaining<RegisterPatientConsumer>();
            
            cfg.UsingRabbitMq((context, configurator) =>
            {
                configurator.Host(config.BrokerCredentials.Host, "/", h =>
                {
                    h.Username(config.BrokerCredentials.Username);
                    h.Password(config.BrokerCredentials.Password);
                });
                
                configurator.ReceiveEndpoint("register-patient", e =>
                {
                    e.UseEntityFrameworkOutbox<PatientDbContext>(context.GetService<IServiceProvider>()!);
                    e.UseTransaction(x =>
                    {
                        x.Timeout = TimeSpan.FromSeconds(90);
                        x.IsolationLevel = IsolationLevel.ReadCommitted;
                    });
                    
                    e.ConfigureConsumer<RegisterPatientConsumer>(context);
                });
                
                configurator.ConfigureEndpoints(context);
            });
        });
    }
}