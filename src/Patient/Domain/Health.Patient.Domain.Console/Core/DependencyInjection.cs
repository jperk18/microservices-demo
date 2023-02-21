using FluentValidation.AspNetCore;
using Health.Patient.Domain.Console.Commands.CreatePatientCommand;
using Health.Patient.Domain.Console.Consumer;
using Health.Patient.Domain.Console.Core.Configuration;
using Health.Patient.Domain.Console.Core.Pipelines;
using Health.Patient.Domain.Console.Core.Services;
using Health.Patient.Domain.Storage.Sql.Core;
using Health.Shared.Domain;
using Health.Shared.Domain.Mediator.Configurations;
using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Health.Patient.Domain.Console.Core;

public static class DependencyInjection
{
    public static void AddDomainServices(this IServiceCollection services, PatientDomainConfiguration config)
    {
        if (config == null || config.PatientStorage == null)
            throw new ApplicationException("Configuration is needed for domain services");

        services.AddFluentValidation(fv =>
            fv.RegisterValidatorsFromAssemblyContaining<CreatePatientCommandValidator>());

        //Add Dependant Database services
        services.AddStorageServices(config.PatientStorage);

        //Add Core services (serialization and Transaction handling)
        var handlerTypes = typeof(Program).Assembly.GetTypes()
            .Where(x => x.GetInterfaces().Any(Shared.Domain.Mediator.DependencyInjection.Handlers.IsHandlerInterface))
            .Where(x => x.Name.EndsWith("Handler"))
            .ToList(); //This assembly Handlers

        services.AddSharedDomainServices(handlerTypes, new List<PipelineConfigurationDto>()
        {
            new (typeof(PatientTransactionPipelineAttribute)){
                CommandHandler = typeof(PatientTransactionCommandDecorator<,>),
                QueryHandler = null //Transaction pipeline not supported in queries
            }
        });
        
        //Services for this application
        services.AddSingleton(config);
        services.AddTransient<IPatientRetrievalService, PatientRetrievalService>();

        services.TryAddSingleton(KebabCaseEndpointNameFormatter.Instance);
        services.AddMassTransit(cfg =>
        {
            cfg.AddConsumersFromNamespaceContaining<RegisterPatientConsumer>();
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
    }
}