﻿using FluentValidation.AspNetCore;
using Health.Patient.Domain.Console.Commands.CreatePatientCommand;
using Health.Patient.Domain.Console.Consumer;
using Health.Patient.Domain.Console.Core.Configuration;
using Health.Patient.Domain.Console.Core.Pipelines;
using Health.Patient.Domain.Console.Core.Services;
using Health.Patient.Domain.Storage.Sql.Core;
using Health.Shared.Domain.Core;
using Health.Shared.Domain.Core.RegistrationHelpers;
using MassTransit;
using MassTransit.Definition;
using MassTransit.RabbitMqTransport;
using MassTransit.Transactions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Health.Patient.Domain.Console.Core;

public static class DependencyInjection
{
    public static void AddDomainServices(this IServiceCollection services, IPatientDomainConfiguration config)
    {
        if (config == null || config.PatientStorageConfiguration == null)
            throw new ApplicationException("Configuration is needed for domain services");

        services.AddFluentValidation(fv =>
            fv.RegisterValidatorsFromAssemblyContaining<CreatePatientCommandValidator>());

        //Add Dependant Database services
        services.AddStorageServices(config.PatientStorageConfiguration);

        //Add Core services (serialization and Transaction handling)
        var handlerTypes = typeof(Program).Assembly.GetTypes()
            .Where(x => x.GetInterfaces().Any(y => Handlers.IsHandlerInterface(y)))
            .Where(x => x.Name.EndsWith("Handler"))
            .ToList(); //This assembly Handlers

        services.AddCoreDomainServices(handlerTypes, new Dictionary<Type, Func<object, Type, Type?>>()
        {
            {
                typeof(PatientTransactionPipelineAttribute), (attribute, assigningInterfaceType) =>
                {
                    if (Handlers.IsCommandHandlerInterface(assigningInterfaceType))
                        return typeof(PatientTransactionCommandDecorator<,>);
                    
                    //Transaction pipeline not supported in queries
                    
                    return null;
                }
            }
        });

        //Services for this application
        services.AddSingleton(config);
        services.AddTransient<IPatientRetrievalService, PatientRetrievalService>();

        services.TryAddSingleton(KebabCaseEndpointNameFormatter.Instance);
        services.AddMassTransit(cfg =>
        {
            cfg.AddConsumersFromNamespaceContaining<RegisterPatientCommandQueryConsumer>();
            cfg.UsingRabbitMq(ConfigureBus);
            
            cfg.AddTransactionalBus();
        });

        services.AddMassTransitHostedService();
    }

    static void ConfigureBus(IBusRegistrationContext context, IRabbitMqBusFactoryConfigurator configurator)
    {
        configurator.ConfigureEndpoints(context);
    }
}