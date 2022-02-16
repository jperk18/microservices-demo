using Automatonymous;
using MassTransit.Saga;

namespace Health.Workflow.Domain.Console.StateMachines;

public class AppointmentState : SagaStateMachineInstance
{
    public Guid CorrelationId { get; set; }
    public string CurrentState { get; set; }
    public Guid PatientId { get; set; }
}