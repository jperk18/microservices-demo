using Health.Shared.Application;
using Health.Shared.Application.Serialization;
using Health.Shared.Domain.Mediator;
using Health.Shared.Domain.Mediator.Configurations;
using Microsoft.Extensions.DependencyInjection;

namespace Health.Shared.Domain;

public static class DependencyInjection
{
    public static void AddCoreDomainServices(this IServiceCollection services, IEnumerable<Type> assemblyHandlerTypes, 
        IEnumerable<IPipelineConfiguration>? additionalPipelinesForHandlers = null,
        IEnumerable<IPipelineConfiguration>? corePipelinesForHandlersOverriders = null)
    {
        services.AddMediatorServices(assemblyHandlerTypes, additionalPipelinesForHandlers, corePipelinesForHandlersOverriders);
        services.AddSerializationServices();
    }
}