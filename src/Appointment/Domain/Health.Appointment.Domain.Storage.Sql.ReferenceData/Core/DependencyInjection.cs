using System.Reflection;
using Health.Appointment.Domain.Storage.Sql.ReferenceData.Core.Configuration;
using Health.Appointment.Domain.Storage.Sql.ReferenceData.Database;
using Health.Appointment.Domain.Storage.Sql.ReferenceData.Repository.Generic;
using Health.Appointment.Domain.Storage.Sql.ReferenceData.Repository.Patient;
using Health.Shared.Domain.Storage.Configuration;
using Health.Shared.Domain.Storage.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.DependencyInjection;

namespace Health.Appointment.Domain.Storage.Sql.ReferenceData.Core;

public static class DependencyInjection
{
    public static void AddReferenceDataStorageServices(this IServiceCollection services, IReferenceDataStorageConfiguration configuration)
    {
        if (configuration == null)
            throw new ApplicationException("Database configuration is required for storage");

        if (configuration.ReferenceDataDatabase.DbType == SqlType.InMemory)
        {
            services.AddDbContext<ReferenceDataDbContext>(options =>
                options.UseInMemoryDatabase("AppointmentDb")
                    .ConfigureWarnings(w => w.Ignore(InMemoryEventId.TransactionIgnoredWarning)));
        }
        else
        {
            if (String.IsNullOrEmpty(configuration.ReferenceDataDatabase.ConnectionString))
                throw new ApplicationException("Database connection string is required for SQL database");

            if (configuration.ReferenceDataDatabase.DbType == SqlType.Postgres)
            {
                services.AddDbContext<ReferenceDataDbContext>(options =>
                    options.UseNpgsql(configuration.ReferenceDataDatabase.ConnectionString, m =>
                    {
                        m.MigrationsAssembly(Assembly.GetExecutingAssembly().GetName().Name);
                        m.MigrationsHistoryTable($"__{nameof(ReferenceDataDbContext)}");
                    }));
            }
        }

        services.AddScoped(typeof(IGenericQueryRepository<>), typeof(GenericReferenceDataQueryRepository<>));
        
        services.AddSingleton(configuration);
        services.AddTransient<IPatientReferenceDataQueryRepository, PatientReferenceDataQueryRepository>();
    }
}
