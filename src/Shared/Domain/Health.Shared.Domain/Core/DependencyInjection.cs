using Health.Nurse.Domain.Console.Core.Serialization;
using Health.Shared.Domain.Core.RegistrationHelpers;
using Microsoft.Extensions.DependencyInjection;

namespace Health.Shared.Domain.Core;

public static class DependencyInjection
{
    public static void AddCoreDomainServices(this IServiceCollection services,IEnumerable<Type> assemblyHandlerTypes, 
        IDictionary<Type, Func<object, Type, Type?>>? additionalPipelinesForHandlers,
        IDictionary<Type, Func<object, Type, Type?>>? corePipelinesForHandlersOverriders = null)
    {
        services.AddHandlers(assemblyHandlerTypes, additionalPipelinesForHandlers, corePipelinesForHandlersOverriders);
        services.AddSingleton<IJsonSerializer, JsonSerializer>();
    }
}