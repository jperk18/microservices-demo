using Health.Patient.Domain.Core.RegistrationHelpers;
using Health.Patient.Domain.Core.Serialization;
using Health.Patient.Domain.Core.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Health.Patient.Domain.Core;

public static class Register
{
    public static void AddDomainServices(this IServiceCollection services, DomainRegistrationConfiguration config)
    {
        services.AddHandlers(config.isInMemoryDbUsage);
        services.AddSingleton<IJsonSerializer, JsonSerializer>();
        services.AddTransient<IRetrievalService, RetrievalService>();
    }
}

public class DomainRegistrationConfiguration
{
    public bool isInMemoryDbUsage { get; set; }
}