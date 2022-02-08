using FluentValidation.AspNetCore;
using Health.Patient.Domain.Commands.CreatePatientCommand;
using Health.Patient.Domain.Core.RegistrationHelpers;
using Health.Patient.Domain.Core.Serialization;
using Health.Patient.Domain.Core.Services;
using Health.Patient.Storage.Core;
using Microsoft.Extensions.DependencyInjection;

namespace Health.Patient.Domain.Core;

public static class Register
{
    public static void AddDomainServices(this IServiceCollection services, DomainRegistrationConfiguration config)
    {
        if (config == null || config.StorageConfiguration == null)
            throw new ApplicationException("Configuration is needed for domain services");

        services.AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<CreatePatientCommandValidator>());
        services.AddHandlers(config.StorageConfiguration.DbType == StorageRegistrationConfiguration.DatabaseType.InMemory);
        services.AddSingleton<IJsonSerializer, JsonSerializer>();
        services.AddTransient<IRetrievalService, RetrievalService>();
        
        //Add Dependant Database services
        services.AddStorageServices(config.StorageConfiguration);
    }
}