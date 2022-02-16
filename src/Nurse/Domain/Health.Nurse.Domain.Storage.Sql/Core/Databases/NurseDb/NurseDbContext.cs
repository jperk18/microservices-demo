using Microsoft.EntityFrameworkCore;

namespace Health.Nurse.Domain.Storage.Sql.Core.Databases.NurseDb;

public class NurseDbContext : Microsoft.EntityFrameworkCore.DbContext
{
#pragma warning disable CS8618
    public NurseDbContext(DbContextOptions<NurseDbContext> options)
#pragma warning restore CS8618
        : base(options)
    {
    }

    public DbSet<NurseDb.Models.Nurse> Nurses { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<NurseDb.Models.Nurse>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.FirstName).IsRequired();
            entity.Property(e => e.LastName).IsRequired();
            entity.Property(e => e.DateOfBirth).IsRequired();
        });
    }
}