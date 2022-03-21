﻿using Health.Shared.Workflow.Processes.Inner.Models;

namespace Health.Shared.Workflow.Processes.Sagas.Appointment;

public interface PatientCheckedIn
{
    Guid AppointmentId { get; }
    PatientCardInformation Patient { get; }
    
    DateTime Timestamp { get; }
}