using MassTransit;
using MassTransit.EntityFrameworkCoreIntegration;
using Microsoft.EntityFrameworkCore;

namespace Health.Appointment.Domain.Storage.Sql.Appointment.Database;

public class AppointmentStateDbContext: SagaDbContext
{
    public AppointmentStateDbContext(DbContextOptions<AppointmentStateDbContext> options)
        : base(options)
    {
    }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        modelBuilder.AddInboxStateEntity();
        modelBuilder.AddOutboxMessageEntity();
        modelBuilder.AddOutboxStateEntity();
    }
    
    protected override IEnumerable<ISagaClassMap> Configurations
    {
        get { yield return new AppointmentStateMap(); }
    }
}