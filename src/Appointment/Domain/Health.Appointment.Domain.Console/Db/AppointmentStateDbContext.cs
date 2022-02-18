using MassTransit.EntityFrameworkCoreIntegration;
using MassTransit.EntityFrameworkCoreIntegration.Mappings;
using Microsoft.EntityFrameworkCore;

namespace Health.Appointment.Domain.Console.Db;

public class AppointmentStateDbContext: 
    SagaDbContext
{
    public AppointmentStateDbContext(DbContextOptions options)
        : base(options)
    {
    }

    protected override IEnumerable<ISagaClassMap> Configurations
    {
        get { yield return new AppointmentStateMap(); }
    }
}