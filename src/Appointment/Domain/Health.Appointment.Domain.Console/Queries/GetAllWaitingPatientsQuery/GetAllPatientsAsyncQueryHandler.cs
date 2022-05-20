using Health.Appointment.Domain.Storage.Sql;
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
        var r = _unitOfWork.AppointmentState.GetAllWaitingPatients();
        
        //TODO: Remove. Using for testing
        var testing = _unitOfWork.PatientReferenceData.GetAll().ToArray();
        
        var patients = r as Guid[] ?? r.ToArray();

        return await Task.FromResult(patients);
    }
}