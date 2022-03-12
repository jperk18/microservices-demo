﻿using MassTransit.EntityFrameworkCoreIntegration.Mappings;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Health.Appointment.Domain.Storage.Sql.Core.Databases.AppointmentState;

public class AppointmentStateMap : 
    SagaClassMap<StateMachines.AppointmentState>
{
    protected override void Configure(EntityTypeBuilder<StateMachines.AppointmentState> entity, ModelBuilder model)
    {
        entity.HasKey(e => e.CorrelationId);
        entity.Property(e => e.PatientId).IsRequired();
        entity.Property(e => e.CurrentState).HasMaxLength(64).IsRequired();
        
        // model.Entity<StateMachines.AppointmentState>(ent =>
        // {
        //     ent.HasKey(e => e.CorrelationId);
        //     ent.Property(e => e.PatientId).IsRequired();
        //     ent.Property(e => e.CurrentState).HasMaxLength(64).IsRequired();
        // });
    }
}