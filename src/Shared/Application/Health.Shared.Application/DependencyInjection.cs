using Health.Shared.Application.Configuration;
using Health.Shared.Application.Services.Serialization;
using Microsoft.Extensions.DependencyInjection;

namespace Health.Shared.Application;

public static class DependencyInjection
{
    public static void AddSharedApplicationServices(this IServiceCollection services)
    {
        services.AddSingleton<IJsonSerializer, JsonSerializer>();
    }
}