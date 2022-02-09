using Health.Patient.Storage.Sql.Core.Databases.PatientDb;
using Health.Patient.Storage.Sql.Core.Repository.Core.Generic;
using Health.Patient.Storage.Sql.Core.Repository.PatientDb;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Health.Patient.Storage.Sql.Core;

public static class DependencyInjection
{
    public static void AddStorageServices(this IServiceCollection services, IStorageConfiguration configuration)
    {
        if (configuration == null)
            throw new ApplicationException("Database configuration is required for storage");

        if (configuration.DbType == StorageConfiguration.DatabaseType.Sql)
        {
            if (String.IsNullOrEmpty(configuration.ConnectionString))
                throw new ApplicationException("Database connection string is required for SQL database");

            services.AddDbContext<PatientDbContext>(options =>
                options.UseInMemoryDatabase("PatientDb"));
        }
        else
        {
            services.AddDbContext<PatientDbContext>(options =>
                options.UseInMemoryDatabase("PatientDb"));
        }

        services.AddTransient(typeof(IGenericQueryRepository<>), typeof(GenericQueryRepository<>));
        services.AddTransient(typeof(IGenericRepository<>), typeof(GenericRepository<>));
        services.AddSingleton(configuration);
        services.AddTransient<IPatientRepository, PatientRepository>();
        services.AddTransient<IPatientUnitOfWork, PatientUnitOfWork>();
    }
}