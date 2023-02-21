using Health.Appointment.Domain.Storage.Sql.Appointment;
using Health.Shared.Domain.Mediator.Decorators;
using Health.Shared.Domain.Mediator.Queries;

namespace Health.Appointment.Domain.Console.Queries.GetAllWaitingPatientsQuery;

[LoggingPipeline]
[ExceptionPipeline]
public sealed class GetAllWaitingPatientsAsyncQueryHandler : IAsyncQueryHandler<GetAllWaitingPatientsQuery, IEnumerable<Guid>>
{
    private readonly IAppointmentRepository _unitOfWork;
    public GetAllWaitingPatientsAsyncQueryHandler(IAppointmentRepository unitOfWork)
    {
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
    }
    
    public async Task<IEnumerable<Guid>> Handle(GetAllWaitingPatientsQuery command)
    {
        var r = await _unitOfWork.GetWaitingPatients();
        return r == null ? Array.Empty<Guid>() : r.ToArray();
    }
}