using Health.Workflow.Domain.Console.StateMachines;
using MassTransit.EntityFrameworkCoreIntegration.Mappings;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Health.Workflow.Domain.Console.Db;

public class AppointmentStateMap : 
    SagaClassMap<AppointmentState>
{
    protected override void Configure(EntityTypeBuilder<AppointmentState> entity, ModelBuilder model)
    {
        entity.Property(x => x.CurrentState).HasMaxLength(64);
        entity.Property(x => x.PatientId);
    }
}