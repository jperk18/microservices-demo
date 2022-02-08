using Health.Patient.Domain.Core.RegistrationHelpers;
using Health.Patient.Domain.Core.Serialization;
using Health.Patient.Domain.Core.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Health.Patient.Domain.Core;

public static class Register
{
    public static void AddDomainServices(this IServiceCollection services, bool isInMemoryDbUsage)
    {
        services.AddHandlers(isInMemoryDbUsage);
        services.AddSingleton<IJsonSerializer, JsonSerializer>();
        services.AddTransient<IRetrievalService, RetrievalService>();
    }
}