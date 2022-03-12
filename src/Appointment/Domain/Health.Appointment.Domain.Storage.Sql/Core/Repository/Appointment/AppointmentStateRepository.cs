using Health.Appointment.Domain.StateMachines;
using Health.Appointment.Domain.Storage.Sql.Core.Databases.AppointmentState;
using Health.Appointment.Domain.Storage.Sql.Core.Repository.Core.Generic;

namespace Health.Appointment.Domain.Storage.Sql.Core.Repository.Appointment;

public class AppointmentStateRepository : GenericRepository<AppointmentState>, IAppointmentStateRepository
{
#pragma warning disable CS0108, CS0114
    private readonly AppointmentStateDbContext _context;
#pragma warning restore CS0108, CS0114

    public AppointmentStateRepository(AppointmentStateDbContext context) : base(context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }
}