using Health.Appointment.Domain.Console.Consumer;
using Health.Appointment.Domain.Console.Core.Configuration;
using Health.Appointment.Domain.Console.Core.Pipelines;
using Health.Appointment.Domain.Storage.UnitOfWorks;
using Health.Shared.Domain;
using Health.Shared.Domain.Mediator.Configurations;
using Microsoft.Extensions.DependencyInjection;

namespace Health.Appointment.Domain.Console.Core;

public static class DependencyInjection
{
    public static void AddDomainServices(this IServiceCollection services, AppointmentDomainConfiguration config)
    {
        if (config?.BrokerCredentials == null)
            throw new ApplicationException("Configuration is needed for domain services");

        //Add Dependant Database services
        services.AddStorageServices(config.AppointmentStorageConfiguration);
        
        //Add Core services (serialization and Transaction handling)
        var handlerTypes = typeof(Program).Assembly.GetTypes()
            .Where(x => x.GetInterfaces().Any(Shared.Domain.Mediator.DependencyInjection.Handlers.IsHandlerInterface))
            .Where(x => x.Name.EndsWith("Handler"))
            .ToList(); //This assembly Handlers

        services.AddSharedDomainServices(handlerTypes, new List<PipelineConfigurationDto>()
        {
            new(typeof(AppointmentTransactionPipelineAttribute)){
                CommandHandler = typeof(AppointmentTransactionCommandDecorator<,>),
                QueryHandler = null //Transaction pipeline not supported in queries
            }
        });

        //Services for this application
        services.AddSingleton(config);
        
        services.AddAppointmentMassTransit(config.BrokerCredentials, typeof(GetAllWaitingPatientsConsumer));
    }
}