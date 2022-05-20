using System.Reflection;
using Health.Appointment.Domain.Storage.Sql.Appointment.Core.Configuration;
using Health.Appointment.Domain.Storage.Sql.Appointment.Database;
using Health.Appointment.Domain.Storage.Sql.Appointment.Repository.AppointmentState;
using Health.Appointment.Domain.Storage.Sql.Appointment.Repository.Generic;
using Health.Shared.Domain.Storage.Configuration;
using Health.Shared.Domain.Storage.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.DependencyInjection;

namespace Health.Appointment.Domain.Storage.Sql.Appointment.Core;

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

        services.AddScoped(typeof(IGenericQueryRepository<>), typeof(GenericAppointmentStateDbQueryRepository<>));
        services.AddScoped(typeof(IGenericRepository<>), typeof(GenericAppointmentStateDbRepository<>));
        
        services.AddSingleton(configuration);
        services.AddTransient<IAppointmentStateRepository, AppointmentStateRepository>();
    }
}