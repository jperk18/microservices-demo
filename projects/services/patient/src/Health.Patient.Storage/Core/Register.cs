using Health.Patient.Storage.Core.Database;
using Health.Patient.Storage.Core.Repository;
using Health.Patient.Storage.Core.Repository.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Health.Patient.Storage.Core;

public static class Register
{
    public static void AddStorageServices(this IServiceCollection services)
    {
        services.AddDbContext<PatientDbContext>(options =>
            options.UseInMemoryDatabase("PatientDb"));
        
        services.AddTransient(typeof(IGenericQueryRepository<>), typeof(GenericQueryRepository<>));
        services.AddTransient(typeof(IGenericRepository<>), typeof(GenericRepository<>));
        services.AddTransient<IPatientRepository, PatientRepository>();
        services.AddTransient<IUnitOfWork, UnitOfWork>();
    }
}