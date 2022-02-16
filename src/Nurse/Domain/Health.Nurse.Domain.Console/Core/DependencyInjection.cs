using FluentValidation.AspNetCore;
using Health.Nurse.Domain.Console.Commands.CreateNurseCommand;
using Health.Nurse.Domain.Console.Consumer;
using Health.Nurse.Domain.Console.Core.RegistrationHelpers;
using Health.Nurse.Domain.Console.Core.Serialization;
using Health.Nurse.Domain.Console.Core.Services;
using Health.Nurse.Domain.Storage.Sql.Core;
using MassTransit;
using MassTransit.Definition;
using MassTransit.RabbitMqTransport;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Health.Nurse.Domain.Console.Core;

public static class DependencyInjection
{
    public static void AddDomainServices(this IServiceCollection services, IDomainConfiguration config)
    {
        if (config == null || config.StorageConfiguration == null)
            throw new ApplicationException("Configuration is needed for domain services");

        services.AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<CreateNurseCommandValidator>());
        services.AddHandlers();
        services.AddSingleton(config);
        services.AddSingleton<IJsonSerializer, JsonSerializer>();
        services.AddTransient<IRetrievalService, RetrievalService>();

        services.TryAddSingleton(KebabCaseEndpointNameFormatter.Instance);
        services.AddMassTransit(cfg =>
        {
            cfg.AddConsumersFromNamespaceContaining<RegisterNurseCommandQueryConsumer>();
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