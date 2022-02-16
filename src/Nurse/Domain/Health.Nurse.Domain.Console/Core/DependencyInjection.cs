using FluentValidation.AspNetCore;
using Health.Nurse.Domain.Console.Commands.CreateNurseCommand;
using Health.Nurse.Domain.Console.Consumer;
using Health.Nurse.Domain.Console.Core.Configuration;
using Health.Nurse.Domain.Storage.Sql.Core;
using Health.Nurse.Domain.Storage.Sql.Core.Databases.NurseDb;
using Health.Shared.Domain.Core;
using Health.Shared.Domain.Core.RegistrationHelpers;
using MassTransit;
using MassTransit.Definition;
using MassTransit.RabbitMqTransport;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Health.Nurse.Domain.Console.Core;

public static class DependencyInjection
{
    public static void AddDomainServices(this IServiceCollection services, INurseDomainConfiguration config)
    {
        if (config == null || config.StorageConfiguration == null)
            throw new ApplicationException("Configuration is needed for domain services");

        services.AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<CreateNurseCommandValidator>());

        var types = typeof(Program).Assembly.GetTypes(); //This assembly's Types
        var handlerTypes= types
            .Where(x => x.GetInterfaces().Any(y => Handlers.IsHandlerInterface(y)))
            .Where(x => x.Name.EndsWith("Handler"))
            .ToList(); //This assembly Handlers
        
        services.AddHandlers(types, handlerTypes);
        
        
        services.AddSingleton(config);

        services.TryAddSingleton(KebabCaseEndpointNameFormatter.Instance);
        services.AddMassTransit(cfg =>
        {
            cfg.AddConsumersFromNamespaceContaining<RegisterNurseCommandQueryConsumer>();
            cfg.UsingRabbitMq(ConfigureBus);
        });

        services.AddMassTransitHostedService();
            
        //Add Dependant Database services
        services.AddStorageServices(config.StorageConfiguration);
        
        //Add Core services (serialization and Transaction handling)
        services.AddCoreDomainServices(typeof(NurseDbContext));
    }
    
    static void ConfigureBus(IBusRegistrationContext context, IRabbitMqBusFactoryConfigurator configurator) {
        configurator.ConfigureEndpoints(context);
    }
}