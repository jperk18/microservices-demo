using Microsoft.EntityFrameworkCore;

namespace Health.Patient.Storage.Sql.Core.Databases.PatientDb;

public class PatientDbContext : Microsoft.EntityFrameworkCore.DbContext 
{
#pragma warning disable CS8618
    public PatientDbContext(DbContextOptions<PatientDbContext> options)
#pragma warning restore CS8618
        : base(options)
    {
    }
    
    public DbSet<Models.Patient> Patients { get; set; }
}