using Microsoft.Extensions.DependencyInjection;

namespace Health.Shared.Application.Serialization;

public static class DependencyInjection
{
    public static void AddSerializationServices(this IServiceCollection services)
    {
        services.AddSingleton<JsonSerializer, JsonSerializerDto>();
    }
}