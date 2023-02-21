using Health.Patient.Domain.Storage.Sql.Core.Configuration;
using Health.Patient.Domain.Storage.Sql.Core.Databases.PatientDb;
using Health.Shared.Domain.Storage.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.DependencyInjection;

namespace Health.Patient.Domain.Storage.Sql.Core;

public static class DependencyInjection
{
    public static void AddStorageServices(this IServiceCollection services, PatientStorageConfiguration configuration)
    {
        if (configuration == null)
            throw new ApplicationException("Database configuration is required for storage");

        if (configuration.PatientDatabase.DbType == SqlType.InMemory)
        {
            services.AddDbContext<PatientDbContext>(options =>
                options.UseInMemoryDatabase("PatientDb")
                    .ConfigureWarnings(w => w.Ignore(InMemoryEventId.TransactionIgnoredWarning)));
        }
        else
        {
            if (String.IsNullOrEmpty(configuration.PatientDatabase.ConnectionString))
                throw new ApplicationException("Database connection string is required for SQL database");

            if (configuration.PatientDatabase.DbType == SqlType.Postgres)
            {
                services.AddEntityFrameworkNpgsql().AddDbContext<PatientDbContext>(opt =>
                    opt.UseNpgsql(configuration.PatientDatabase.ConnectionString));
            }
        }
        
        services.AddSingleton(configuration);
        services.AddTransient<IPatientUnitOfWork, PatientUnitOfWork>();
    }
}