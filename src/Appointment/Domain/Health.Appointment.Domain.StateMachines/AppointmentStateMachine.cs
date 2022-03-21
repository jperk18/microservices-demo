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
                .Then(context => { context.Instance.PatientId = context.Data.Patient.Id; })
                .TransitionTo(PatientAwaitingNurse)
        );

        During(PatientAwaitingNurse,
            Ignore(PatientCheckedIn),
            When(AssignedNurseToPatient)
                .Then(context => { context.Instance.NurseId = context.Data.Nurse.Id; })
                .TransitionTo(VitalCheckExaminationInProgress)
        );

        During(VitalCheckExaminationInProgress,
            Ignore(PatientCheckedIn),
            Ignore(AssignedNurseToPatient),
            When(RecordedPatientVitals)
                .TransitionTo(PatientWaitingForDoctor)
        );

        During(PatientWaitingForDoctor,
            Ignore(PatientCheckedIn),
            Ignore(AssignedNurseToPatient),
            Ignore(RecordedPatientVitals),
            When(AssignedDoctorToPatient)
                .Then(context => { context.Instance.DoctorId = context.Data.Doctor.Id; })
                .TransitionTo(AppointmentInProgress)
        );

        During(AppointmentInProgress,
            Ignore(PatientCheckedIn),
            Ignore(AssignedNurseToPatient),
            Ignore(RecordedPatientVitals),
            Ignore(AssignedDoctorToPatient),
            When(RecordHealthAndAppointmentEnded)
                .TransitionTo(AppointmentCompleted)
                .Finalize()
        );

        DuringAny(
            When(PatientCheckedIn)
                .Then(context => { context.Instance.PatientId = context.Data.Patient.Id; }),
            When(RecordHealthAndAppointmentEnded)
                .Finalize()
        );

        SetCompletedWhenFinalized();
    }

    public State PatientAwaitingNurse { get; private set; }
    public State VitalCheckExaminationInProgress { get; private set; }
    public State PatientWaitingForDoctor { get; private set; }
    public State AppointmentInProgress { get; private set; }
    public State AppointmentCompleted { get; private set; }
    public Event<RecordHealthAndAppointmentEnded> RecordHealthAndAppointmentEnded { get; private set; }
    public Event<AssignedNurseToPatient> AssignedNurseToPatient { get; private set; }
    public Event<AssignedDoctorToPatient> AssignedDoctorToPatient { get; private set; }
    public Event<RecordedPatientVitals> RecordedPatientVitals { get; private set; }
    public Event<PatientCheckedIn> PatientCheckedIn { get; private set; }
}