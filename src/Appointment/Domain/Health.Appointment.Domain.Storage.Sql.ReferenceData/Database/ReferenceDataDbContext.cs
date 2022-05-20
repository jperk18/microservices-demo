using Health.Appointment.Domain.Storage.Sql.ReferenceData.Database.Entities;
using Microsoft.EntityFrameworkCore;

namespace Health.Appointment.Domain.Storage.Sql.ReferenceData.Database;

public class ReferenceDataDbContext : DbContext
{
    public ReferenceDataDbContext(DbContextOptions<ReferenceDataDbContext> options)
        : base(options)
    {
    }
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Patient>()
            .HasKey(x => x.PatientId);
    }
    
    //entities
    public DbSet<Patient>? Patients { get; set; }
}