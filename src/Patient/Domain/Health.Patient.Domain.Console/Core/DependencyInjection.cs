using FluentValidation.AspNetCore;
using Health.Patient.Domain.Console.Commands.CreatePatientCommand;
using Health.Patient.Domain.Console.Consumer;
using Health.Patient.Domain.Console.Core.RegistrationHelpers;
using Health.Patient.Domain.Console.Core.Serialization;
using Health.Patient.Domain.Console.Core.Services;
using Health.Patient.Domain.Storage.Sql.Core;
using MassTransit;
using MassTransit.Definition;
using MassTransit.RabbitMqTransport;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Health.Patient.Domain.Console.Core;

public static class DependencyInjection
{
    public static void AddDomainServices(this IServiceCollection services, IDomainConfiguration config)
    {
        if (config == null || config.StorageConfiguration == null)
            throw new ApplicationException("Configuration is needed for domain services");

        services.AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<CreatePatientCommandValidator>());
        services.AddHandlers();
        services.AddSingleton(config);
        services.AddSingleton<IJsonSerializer, JsonSerializer>();
        services.AddTransient<IPatientRetrievalService, PatientRetrievalService>();

        services.TryAddSingleton(KebabCaseEndpointNameFormatter.Instance);
        services.AddMassTransit(cfg =>
        {
            cfg.AddConsumersFromNamespaceContaining<RegisterPatientCommandQueryConsumer>();
            cfg.UsingRabbitMq(ConfigureBus);
        });

        services.AddMassTransitHostedService();
            
        //Add Dependant Database services
        services.AddStorageServices(config.StorageConfiguration);
    }
    
    static void ConfigureBus(IBusRegistrationContext context, IRabbitMqBusFactoryConfigurator configurator) {
        configurator.ConfigureEndpoints(context);
    }
}