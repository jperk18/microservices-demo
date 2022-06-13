using Health.Shared.Domain.Core.Configurations;
using Health.Shared.Domain.Core.RegistrationHelpers;
using Health.Shared.Domain.Core.Serialization;
using Microsoft.Extensions.DependencyInjection;

namespace Health.Shared.Domain.Core;

public static class DependencyInjection
{
    public static void AddCoreDomainServices(this IServiceCollection services, IEnumerable<Type> assemblyHandlerTypes, 
        IEnumerable<IPipelineConfiguration>? additionalPipelinesForHandlers = null,
        IEnumerable<IPipelineConfiguration>? corePipelinesForHandlersOverriders = null)
    {
        services.AddHandlers(assemblyHandlerTypes, additionalPipelinesForHandlers, corePipelinesForHandlersOverriders);
        services.AddSingleton<IJsonSerializer, JsonSerializer>();
    }
}