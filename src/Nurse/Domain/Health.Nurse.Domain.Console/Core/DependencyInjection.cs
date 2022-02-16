﻿using FluentValidation.AspNetCore;
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
        if (config == null || config.NurseStorageConfiguration == null)
            throw new ApplicationException("Configuration is needed for domain services");

        services.AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<CreateNurseCommandValidator>());

        //Add Dependant Database services
        services.AddStorageServices(config.NurseStorageConfiguration);
        
        //Add Core services (serialization and Transaction handling)
        var handlerTypes = typeof(Program).Assembly.GetTypes()
            .Where(x => x.GetInterfaces().Any(y => Handlers.IsHandlerInterface(y)))
            .Where(x => x.Name.EndsWith("Handler"))
            .ToList(); //This assembly Handlers

        services.AddCoreDomainServices(handlerTypes, typeof(NurseDbContext));

        //Services for this application
        services.AddSingleton(config);
        services.TryAddSingleton(KebabCaseEndpointNameFormatter.Instance);
        services.AddMassTransit(cfg =>
        {
            cfg.AddConsumersFromNamespaceContaining<RegisterNurseCommandQueryConsumer>();
            cfg.UsingRabbitMq(ConfigureBus);
        });

        services.AddMassTransitHostedService();
    }
    
    static void ConfigureBus(IBusRegistrationContext context, IRabbitMqBusFactoryConfigurator configurator) {
        configurator.ConfigureEndpoints(context);
    }
}