using MassTransit.EntityFrameworkCoreIntegration;
using MassTransit.EntityFrameworkCoreIntegration.Mappings;
using Microsoft.EntityFrameworkCore;

namespace Health.Appointment.Domain.Storage.Sql.Appointment.Database;

public class AppointmentStateDbContext: 
    SagaDbContext
{
    public AppointmentStateDbContext(DbContextOptions<AppointmentStateDbContext> options)
        : base(options)
    {
    }

    protected override IEnumerable<ISagaClassMap> Configurations
    {
        get { yield return new AppointmentStateMap(); }
    }
}