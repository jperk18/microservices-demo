using Health.Patient.Storage.Core.Database;
using Health.Patient.Storage.Core.Repository.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Health.Patient.Storage.Core;

public static class Register
{
    public static void AddStorageServices(this IServiceCollection services, StorageRegistrationConfiguration configuration)
    {
        if(configuration == null)
            throw new ApplicationException("Database configuration is required for storage");

        if (configuration.DbType == StorageRegistrationConfiguration.DatabaseType.Sql)
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
        services.AddTransient<IPatientRepository, PatientRepository>();
        services.AddTransient<IUnitOfWork, UnitOfWork>();
    }
}