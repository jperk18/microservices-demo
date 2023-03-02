using MassTransit;
using Microsoft.EntityFrameworkCore;

namespace Health.Patient.Domain.Storage.Sql.Databases.PatientDb;

public class PatientDbContext : Microsoft.EntityFrameworkCore.DbContext
{
#pragma warning disable CS8618
    public PatientDbContext(DbContextOptions<PatientDbContext> options)
#pragma warning restore CS8618
        : base(options)
    {
    }

    public DbSet<Models.Patient> Patients { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        modelBuilder.Entity<Models.Patient>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.FirstName).IsRequired();
            entity.Property(e => e.LastName).IsRequired();
            entity.Property(e => e.DateOfBirth).IsRequired();
        });
        
        modelBuilder.AddInboxStateEntity();
        modelBuilder.AddOutboxMessageEntity();
        modelBuilder.AddOutboxStateEntity();
    }
}