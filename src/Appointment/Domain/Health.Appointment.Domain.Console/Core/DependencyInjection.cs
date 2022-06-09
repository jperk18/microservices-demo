using Health.Appointment.Domain.Console.Consumer;
using Health.Appointment.Domain.Console.Core.Configuration;
using Health.Appointment.Domain.Console.Core.Pipelines;
using Health.Appointment.Domain.Storage.UnitOfWorks.Core;
using Health.Shared.Domain.Core;
using Health.Shared.Domain.Core.Configurations;
using Health.Shared.Domain.Core.RegistrationHelpers;
using Microsoft.Extensions.DependencyInjection;

namespace Health.Appointment.Domain.Console.Core;

public static class DependencyInjection
{
    public static void AddDomainServices(this IServiceCollection services, IAppointmentDomainConfiguration config)
    {
        if (config == null || config.BrokerCredentials == null)
            throw new ApplicationException("Configuration is needed for domain services");

        //Add Dependant Database services
        services.AddUnitsOfWork(config.AppointmentStorageConfiguration, config.ReferenceDataStorageConfiguration);
        
        //Add Core services (serialization and Transaction handling)
        var handlerTypes = typeof(Program).Assembly.GetTypes()
            .Where(x => x.GetInterfaces().Any(y => Handlers.IsHandlerInterface(y)))
            .Where(x => x.Name.EndsWith("Handler"))
            .ToList(); //This assembly Handlers

        services.AddCoreDomainServices(handlerTypes, new List<PipelineConfigurationDto>()
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