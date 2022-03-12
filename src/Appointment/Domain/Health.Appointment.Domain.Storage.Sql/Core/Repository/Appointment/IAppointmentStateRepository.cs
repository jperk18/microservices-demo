using Health.Appointment.Domain.StateMachines;
using Health.Appointment.Domain.Storage.Sql.Core.Repository.Core.Generic;

namespace Health.Appointment.Domain.Storage.Sql.Core.Repository.Appointment;

public interface IAppointmentStateRepository : IGenericRepository<AppointmentState>
{
    
}