using Health.Nurse.Domain.Console.Core.Serialization;
using Microsoft.Extensions.DependencyInjection;

namespace Health.Shared.Domain.Core;

public static class DependencyInjection
{
    public static void AddCoreDomainServices(this IServiceCollection services)
    {
        services.AddSingleton<IJsonSerializer, JsonSerializer>();
    }
}