using Health.Nurse.Domain.Console.Core.Serialization;
using Health.Shared.Domain.Core.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Health.Shared.Domain.Core;

public static class DependencyInjection
{
    public static void AddCoreDomainServices(this IServiceCollection services, Type dbContextType)
    {
        services.AddSingleton<IJsonSerializer, JsonSerializer>();
        services.AddTransient<IDbTransactionContextType>(x => new DbTransactionContextType(dbContextType));
    }
}