using Health.Patient.Domain.Core.Serialization;
using Health.Patient.Domain.Core.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Health.Patient.Domain.Core.Registration;

public static class DomainRegistration
{
    public static void AddDomainServices(this IServiceCollection services)
    {
        services.AddHandlers();
        services.AddSingleton<IJsonSerializer, JsonSerializer>();
        services.AddTransient<IRetrievalService, RetrievalService>();
    }
}