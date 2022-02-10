using FluentValidation.AspNetCore;
using Health.Patient.Domain.Commands.CreatePatientCommand;
using Health.Patient.Domain.Consumer;
using Health.Patient.Domain.Core.RegistrationHelpers;
using Health.Patient.Domain.Core.Serialization;
using Health.Patient.Domain.Core.Services;
using Health.Patient.Storage.Sql.Core;
using MassTransit;
using MassTransit.Definition;
using MassTransit.RabbitMqTransport;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Health.Patient.Domain.Core;

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
        services.AddTransient<IRetrievalService, RetrievalService>();

        services.TryAddSingleton(KebabCaseEndpointNameFormatter.Instance);
        services.AddMassTransit(cfg =>
        {
            cfg.AddConsumersFromNamespaceContaining<CreatePatientCommandConsumer>();
            cfg.UsingRabbitMq(ConfigureBus);
            
            cfg.AddRequestClient<CreatePatientCommand>();
        });

        services.AddMassTransitHostedService();
            
        //Add Dependant Database services
        services.AddStorageServices(config.StorageConfiguration);
    }
    
    static void ConfigureBus(IBusRegistrationContext context, IRabbitMqBusFactoryConfigurator configurator) {
        configurator.ConfigureEndpoints(context);
    }
}