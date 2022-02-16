using Automatonymous;
using Health.Workflow.Shared.Processes;
using Health.Workflow.Shared.Processes.Appointment;
using MassTransit;

namespace Health.Workflow.Domain.Console.StateMachines;

public class AppointmentStateMachine : MassTransitStateMachine<AppointmentState>
{
    public AppointmentStateMachine()
    {
        Event(() => PatientCheckedIn, x => x.CorrelateById(m => m.Message.AppointmentId));
        
        InstanceState(x => x.CurrentState);
        
        Initially(
            When(PatientCheckedIn)
                .Then(context =>
                {
                    context.Instance.PatientId = context.Data.Patient.Id;
                })
                .TransitionTo(PatientCheckedInAwaitingNurse)
        );
        
        During(PatientCheckedInAwaitingNurse,
            Ignore(PatientCheckedIn));

        DuringAny(
            When(PatientCheckedIn)
                .Then(context =>
                {
                    context.Instance.PatientId = context.Data.Patient.Id;
                })
            );
    }
    
    public State PatientCheckedInAwaitingNurse { get; private set; }
    public Event<IPatientCheckedIn> PatientCheckedIn { get; private set; }
    public Event<GetWaitingPatientsForNurses> GetAllWaitingPatientWaitingForNurses { get; private set; }
}