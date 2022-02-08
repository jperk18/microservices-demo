using Microsoft.EntityFrameworkCore;

namespace Health.Patient.Storage.Core.Database;

public class PatientDbContext : Microsoft.EntityFrameworkCore.DbContext 
{
    public PatientDbContext(DbContextOptions<PatientDbContext> options)
        : base(options)
    {
    }
    
    public DbSet<Models.Patient> Patients { get; set; }
}