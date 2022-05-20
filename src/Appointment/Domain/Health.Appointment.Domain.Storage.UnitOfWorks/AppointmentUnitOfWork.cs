using Health.Appointment.Domain.Storage.Sql.Appointment.Database;
using Health.Appointment.Domain.Storage.Sql.Appointment.Repository.AppointmentState;
using Health.Appointment.Domain.Storage.Sql.ReferenceData.Database;
using Health.Appointment.Domain.Storage.Sql.ReferenceData.Repository.Patient;

namespace Health.Appointment.Domain.Storage.UnitOfWorks;

public class AppointmentUnitOfWork : IAppointmentUnitOfWork
{
    private readonly AppointmentStateDbContext _context;
    private readonly ReferenceDataDbContext _contextRef;

    public AppointmentUnitOfWork(AppointmentStateDbContext context, ReferenceDataDbContext contextRef)
    {
        _context = context;
        _contextRef = contextRef;
        AppointmentState = new AppointmentStateRepository(_context);
        PatientReferenceData = new PatientReferenceDataQueryRepository(_contextRef);
        //Add additional table repos here
    }

    public IAppointmentStateRepository AppointmentState { get; private set; }
    public IPatientReferenceDataQueryRepository PatientReferenceData { get; }

    public async Task<int> Complete()
    {
        return await _context.SaveChangesAsync();
    }

    public void Dispose()
    {
        _context.Dispose();
    }
}