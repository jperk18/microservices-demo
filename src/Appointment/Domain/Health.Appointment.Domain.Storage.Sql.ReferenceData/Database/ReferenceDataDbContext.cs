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
        
        modelBuilder.Entity<Entities.Nurse>()
            .HasKey(x => x.NurseId);
    }
    
    //entities
    public DbSet<Patient>? Patients { get; set; }
    public DbSet<Entities.Nurse>? Nurses { get; set; }
}