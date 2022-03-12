using System.Reflection;
using Health.Appointment.Domain.Storage.Sql.Core.Configuration;
using Health.Appointment.Domain.Storage.Sql.Core.Configuration.Inner;
using Health.Appointment.Domain.Storage.Sql.Core.Databases.AppointmentState;
using Health.Appointment.Domain.Storage.Sql.Core.Repository.Appointment;
using Health.Appointment.Domain.Storage.Sql.Core.Repository.Core.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.DependencyInjection;

namespace Health.Appointment.Domain.Storage.Sql.Core;

public static class DependencyInjection
{
    public static void AddStorageServices(this IServiceCollection services, IAppointmentStorageConfiguration configuration)
    {
        if (configuration == null)
            throw new ApplicationException("Database configuration is required for storage");

        if (configuration.AppointmentStateDatabase.DbType == SqlType.InMemory)
        {
            services.AddDbContext<AppointmentStateDbContext>(options =>
                options.UseInMemoryDatabase("AppointmentDb")
                    .ConfigureWarnings(w => w.Ignore(InMemoryEventId.TransactionIgnoredWarning)));
        }
        else
        {
            if (String.IsNullOrEmpty(configuration.AppointmentStateDatabase.ConnectionString))
                throw new ApplicationException("Database connection string is required for SQL database");

            if (configuration.AppointmentStateDatabase.DbType == SqlType.Postgres)
            {
                services.AddDbContext<AppointmentStateDbContext>(options =>
                    options.UseNpgsql(configuration.AppointmentStateDatabase.ConnectionString, m =>
                    {
                        m.MigrationsAssembly(Assembly.GetExecutingAssembly().GetName().Name);
                        m.MigrationsHistoryTable($"__{nameof(AppointmentStateDbContext)}");
                    }));
            }
        }

        services.AddTransient(typeof(IGenericQueryRepository<>), typeof(GenericQueryRepository<>));
        services.AddTransient(typeof(IGenericRepository<>), typeof(GenericRepository<>));
        services.AddSingleton(configuration);
        services.AddTransient<IAppointmentStateRepository, AppointmentStateRepository>();
        services.AddTransient<IUnitOfWork, UnitOfWork>();
    }
}