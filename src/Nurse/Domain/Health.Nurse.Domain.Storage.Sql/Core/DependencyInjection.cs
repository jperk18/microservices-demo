using Health.Nurse.Domain.Storage.Sql.Core.Databases.NurseDb;
using Health.Nurse.Domain.Storage.Sql.Core.Repository.Core.Generic;
using Health.Nurse.Domain.Storage.Sql.Core.Repository.NurseDb;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.DependencyInjection;

namespace Health.Nurse.Domain.Storage.Sql.Core;

public static class DependencyInjection
{
    public static void AddStorageServices(this IServiceCollection services, IStorageConfiguration configuration)
    {
        if (configuration == null)
            throw new ApplicationException("Database configuration is required for storage");

        if (configuration.PatientDatabase.DbType == SqlType.InMemory)
        {
            services.AddDbContext<NurseDbContext>(options =>
                options.UseInMemoryDatabase("NurseDb")
                    .ConfigureWarnings(w => w.Ignore(InMemoryEventId.TransactionIgnoredWarning)));
        }
        else
        {
            if (String.IsNullOrEmpty(configuration.PatientDatabase.ConnectionString))
                throw new ApplicationException("Database connection string is required for SQL database");

            if (configuration.PatientDatabase.DbType == SqlType.Postgres)
            {
                services.AddDbContext<NurseDbContext>(options =>
                    options.UseNpgsql(configuration.PatientDatabase.ConnectionString));
            }
        }

        services.AddTransient(typeof(IGenericQueryRepository<>), typeof(GenericQueryRepository<>));
        services.AddTransient(typeof(IGenericRepository<>), typeof(GenericRepository<>));
        services.AddSingleton(configuration);
        services.AddTransient<INurseRepository, NurseRepository>();
        services.AddTransient<IUnitOfWork, UnitOfWork>();
    }
}