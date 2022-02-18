using Automatonymous;
using Health.Shared.Workflow.Processes.Sagas.Appointment;

namespace Health.Appointment.Domain.Console.StateMachines;

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