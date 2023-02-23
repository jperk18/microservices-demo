using Health.Nurse.Domain.Storage.Sql.Configuration;
using Health.Nurse.Domain.Storage.Sql.Core.Databases.NurseDb;
using Health.Nurse.Domain.Storage.Sql.Databases.NurseDb;
using Health.Shared.Domain.Storage.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.DependencyInjection;

namespace Health.Nurse.Domain.Storage.Sql;

public static class DependencyInjection
{
    public static void AddStorageServices(this IServiceCollection services, NurseStorageConfiguration configuration)
    {
        if (configuration == null)
            throw new ApplicationException("Database configuration is required for storage");

        if (configuration.NurseDatabase.DbType == SqlType.InMemory)
        {
            services.AddDbContext<NurseDbContext>(options =>
                options.UseInMemoryDatabase("NurseDb")
                    .ConfigureWarnings(w => w.Ignore(InMemoryEventId.TransactionIgnoredWarning)));
        }
        else
        {
            if (String.IsNullOrEmpty(configuration.NurseDatabase.ConnectionString))
                throw new ApplicationException("Database connection string is required for SQL database");

            if (configuration.NurseDatabase.DbType == SqlType.Postgres)
            {
                services.AddDbContext<NurseDbContext>(options =>
                    options.UseNpgsql(configuration.NurseDatabase.ConnectionString));
            }
        }
        
        services.AddSingleton(configuration);
        services.AddTransient<INurseRepository, NurseRepository>();
    }
}