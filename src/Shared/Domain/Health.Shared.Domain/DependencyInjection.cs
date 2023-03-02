using Health.Shared.Application;
using Health.Shared.Domain.Mediator;
using Health.Shared.Domain.Mediator.Configurations;
using Health.Shared.Domain.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Health.Shared.Domain;

public static class DependencyInjection
{
    public static void AddSharedDomainServices(this IServiceCollection services, IEnumerable<Type>? assemblyHandlerTypes = null, 
        IEnumerable<PipelineConfiguration>? additionalPipelinesForHandlers = null,
        IEnumerable<PipelineConfiguration>? corePipelinesForHandlersOverriders = null)
    {
        services.AddSharedApplicationServices();
        
        //services.AddValidatorsFromAssemblyContaining<DependencyInjection>();

        services.AddScoped(typeof(IFluentValidationService<>), typeof(FluentValidationService<>));
        
        services.AddMediatorServices(assemblyHandlerTypes, additionalPipelinesForHandlers, corePipelinesForHandlersOverriders);
    }
}