using Automatonymous;
using Health.Shared.Workflow.Processes.Sagas.Appointment;

namespace Health.Appointment.Domain.StateMachines;

public class AppointmentStateMachine : MassTransitStateMachine<AppointmentState>
{
#pragma warning disable CS8618
    public AppointmentStateMachine()
#pragma warning restore CS8618
    {
        Event(() => ScheduleAppointment, e =>
        {
            e.CorrelateById(m => m.Message.AppointmentId);
            
            // e.SelectId(x => NewId.NextGuid());
            //
            // e.InsertOnInitial = true;
            // e.SetSagaFactory(context => new AppointmentState
            // {
            //     CorrelationId = context.CorrelationId ?? NewId.NextGuid(),
            //     //AppointmentId = context.Message.AppointmentId,
            // });
        });
        
        Event(() => PatientCheckedIn, e => e.CorrelateById(m => m.Message.AppointmentId));
        Event(() => AssignedNurseForAppointment, x => x.CorrelateById(m => m.Message.AppointmentId));

        InstanceState(x => x.CurrentState);

        Initially(
            When(ScheduleAppointment)
                .Then(context =>
                {
                    context.Instance.CorrelationId = context.Data.AppointmentId;
                    context.Instance.PatientId = context.Data.PatientId;
                })
                .TransitionTo(AppointmentScheduled)
        );

        During(AppointmentScheduled,
            Ignore(AssignedNurseForAppointment),

            When(PatientCheckedIn)
                .TransitionTo(PatientAwaitingNurse)
        );
        
        During(PatientAwaitingNurse,
            Ignore(PatientCheckedIn),
            When(AssignedNurseForAppointment)
                .Then(context => { context.Instance.NurseId = context.Data.NurseId; })
                .TransitionTo(VitalCheckExaminationInProgress)
        );
        
        // During(VitalCheckExaminationInProgress,
        //     Ignore(PatientCheckedIn),
        //     Ignore(AssignedNurseToPatient),
        //     When(RecordedPatientVitals)
        //         .TransitionTo(PatientWaitingForDoctor)
        // );
        //
        // During(PatientWaitingForDoctor,
        //     Ignore(PatientCheckedIn),
        //     Ignore(AssignedNurseToPatient),
        //     Ignore(RecordedPatientVitals),
        //     When(AssignedDoctorToPatient)
        //         .Then(context => { context.Instance.DoctorId = context.Data.Doctor.Id; })
        //         .TransitionTo(AppointmentInProgress)
        // );
        //
        // During(AppointmentInProgress,
        //     Ignore(PatientCheckedIn),
        //     Ignore(AssignedNurseToPatient),
        //     Ignore(RecordedPatientVitals),
        //     Ignore(AssignedDoctorToPatient),
        //     When(RecordHealthAndAppointmentEnded)
        //         .TransitionTo(AppointmentCompleted)
        //         .Finalize()
        // );
        //
        // DuringAny(
        //     When(PatientCheckedIn)
        //         .Then(context => { context.Instance.PatientId = context.Data.Patient.Id; }),
        //     When(RecordHealthAndAppointmentEnded)
        //         .Finalize()
        // );

        SetCompletedWhenFinalized();
    }

    public State AppointmentScheduled { get; private set; }
    public State PatientAwaitingNurse { get; private set; }
    public State VitalCheckExaminationInProgress { get; private set; }
    
    public Event<AssignedNurseForAppointment> AssignedNurseForAppointment { get; private set; }
    public Event<PatientCheckedIn> PatientCheckedIn { get; private set; }
    public Event<ScheduleAppointment> ScheduleAppointment { get; private set; }
}