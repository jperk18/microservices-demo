using Health.Appointment.Domain.Storage.UnitOfWorks;
using Health.Shared.Domain.Core.Decorators;
using Health.Shared.Domain.Queries.Core;

namespace Health.Appointment.Domain.Console.Queries.GetAllWaitingPatientsQuery;

[LoggingPipeline]
[ExceptionPipeline]
public sealed class GetAllWaitingPatientsAsyncQueryHandler : IAsyncQueryHandler<GetAllWaitingPatientsQuery, IEnumerable<Guid>>
{
    private readonly IAppointmentUnitOfWork _unitOfWork;
    public GetAllWaitingPatientsAsyncQueryHandler(IAppointmentUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
    }
    
    public async Task<IEnumerable<Guid>> Handle(GetAllWaitingPatientsQuery command)
    {
        var r = await _unitOfWork.AppointmentState.GetWaitingPatients();
        return r == null ? Array.Empty<Guid>() : r.ToArray();
    }
}