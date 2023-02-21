using Health.Shared.Application.Serialization;
using Microsoft.Extensions.DependencyInjection;

namespace Health.Shared.Application;

public static class DependencyInjection
{
    public static void AddSharedApplicationServices(this IServiceCollection services)
    {
        services.AddSerializationServices();
    }
}