using Automatonymous;

namespace Health.Appointment.Domain.StateMachines;

public class AppointmentState : SagaStateMachineInstance
{
    public Guid CorrelationId { get; set; }
    public string? CurrentState { get; set; }
    public Guid PatientId { get; set; }
    public Guid? NurseId { get; set; }
    public Guid? DoctorId { get; set; }
}