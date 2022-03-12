using Automatonymous;
using Health.Shared.Workflow.Processes.Sagas.Appointment;

namespace Health.Appointment.Domain.StateMachines;

public class AppointmentStateMachine : MassTransitStateMachine<AppointmentState>
{
#pragma warning disable CS8618
    public AppointmentStateMachine()
#pragma warning restore CS8618
    {
        Event(() => PatientCheckedIn, x => x.CorrelateById(m => m.Message.AppointmentId));
        
        InstanceState(x => x.CurrentState);
        
        Initially(
            When(PatientCheckedIn)
                .Then(context =>
                {
                    context.Instance.PatientId = context.Data.Patient.PatientId;
                })
                .TransitionTo(PatientCheckedInAwaitingNurse)
        );
        
        During(PatientCheckedInAwaitingNurse,
            Ignore(PatientCheckedIn));

        DuringAny(
            When(PatientCheckedIn)
                .Then(context =>
                {
                    context.Instance.PatientId = context.Data.Patient.PatientId;
                })
            );
    }
    
    public State PatientCheckedInAwaitingNurse { get; private set; }
    public Event<PatientCheckedIn> PatientCheckedIn { get; private set; }
    public Event<GetWaitingPatientsForNurses> GetAllWaitingPatientWaitingForNurses { get; private set; }
}