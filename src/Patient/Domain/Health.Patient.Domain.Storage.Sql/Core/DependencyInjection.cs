﻿using Health.Patient.Domain.Storage.Sql.Core.Configuration;
using Health.Patient.Domain.Storage.Sql.Core.Configuration.Inner;
using Health.Patient.Domain.Storage.Sql.Core.Databases.PatientDb;
using Health.Patient.Domain.Storage.Sql.Core.Repository.Core.Generic;
using Health.Patient.Domain.Storage.Sql.Core.Repository.PatientDb;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.DependencyInjection;

namespace Health.Patient.Domain.Storage.Sql.Core;

public static class DependencyInjection
{
    public static void AddStorageServices(this IServiceCollection services, IPatientStorageConfiguration configuration)
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

        services.AddTransient(typeof(IGenericQueryRepository<>), typeof(GenericQueryRepository<>));
        services.AddTransient(typeof(IGenericRepository<>), typeof(GenericRepository<>));
        services.AddSingleton(configuration);
        services.AddTransient<IPatientRepository, PatientRepository>();
        services.AddTransient<IPatientUnitOfWork, PatientUnitOfWork>();
    }
}