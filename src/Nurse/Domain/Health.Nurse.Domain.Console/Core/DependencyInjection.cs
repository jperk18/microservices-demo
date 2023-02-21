using FluentValidation.AspNetCore;
using Health.Nurse.Domain.Console.Commands.CreateNurseCommand;
using Health.Nurse.Domain.Console.Consumer;
using Health.Nurse.Domain.Console.Core.Configuration;
using Health.Nurse.Domain.Console.Core.Pipelines;
using Health.Nurse.Domain.Storage.Sql.Core;
using Health.Shared.Domain;
using Health.Shared.Domain.Mediator.Configurations;
using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Health.Nurse.Domain.Console.Core;

public static class DependencyInjection
{
    public static void AddDomainServices(this IServiceCollection services, INurseDomainConfiguration config)
    {
        if (config == null || config.NurseStorage == null)
            throw new ApplicationException("Configuration is needed for domain services");

        services.AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<CreateNurseCommandValidator>());

        //Add Dependant Database services
        services.AddStorageServices(config.NurseStorage);
        
        //Add Core services (serialization and Transaction handling)
        var handlerTypes = typeof(Program).Assembly.GetTypes()
            .Where(x => x.GetInterfaces().Any(Shared.Domain.Mediator.DependencyInjection.Handlers.IsHandlerInterface))
            .Where(x => x.Name.EndsWith("Handler"))
            .ToList(); //This assembly Handlers

        services.AddCoreDomainServices(handlerTypes, new List<PipelineConfigurationDto>()
        {
            new(typeof(NurseTransactionPipelineAttribute)){
                CommandHandler = typeof(NurseTransactionCommandDecorator<,>),
                QueryHandler = null //Transaction pipeline not supported in queries
            }
        });

        //Services for this application
        services.AddSingleton(config);
        services.TryAddSingleton(KebabCaseEndpointNameFormatter.Instance);
        services.AddMassTransit(cfg =>
        {
            cfg.AddConsumersFromNamespaceContaining<RegisterNurseConsumer>();
            cfg.UsingRabbitMq(ConfigureBus);
            
            cfg.AddTransactionalBus();
        });
    }

    
    
    static void ConfigureBus(IBusRegistrationContext context, IRabbitMqBusFactoryConfigurator configurator) {
        configurator.ReceiveEndpoint("register-nurse", (ctx) =>
        {
            ctx.ConfigureConsumer<RegisterNurseConsumer>(context);
        });
        
        configurator.ConfigureEndpoints(context);
    }
}