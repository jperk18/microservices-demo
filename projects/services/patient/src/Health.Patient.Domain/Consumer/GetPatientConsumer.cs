using Health.Patient.Domain.Commands.Core;
using Health.Patient.Domain.Commands.CreatePatientCommand;
using Health.Patient.Domain.Core.Exceptions;
using Health.Patient.Domain.Core.Models;
using Health.Patient.Domain.Queries.Core;
using Health.Patient.Domain.Queries.GetAllPatientsQuery;
using Health.Patient.Domain.Queries.GetPatientQuery;
using MassTransit;

namespace Health.Patient.Domain.Consumer;

public class GetPatientConsumer : IConsumer<GetPatientQuery>
{
    private readonly IQueryHandler<GetPatientQuery, PatientRecord> _cmdHandler;

    public GetPatientConsumer(IQueryHandler<GetPatientQuery, PatientRecord> cmdHandler)
    {
        _cmdHandler = cmdHandler ?? throw new ArgumentNullException(nameof(cmdHandler));
    }
    public async Task Consume(ConsumeContext<GetPatientQuery> context)
    {
        try
        {
            var result = await _cmdHandler.Handle(context.Message);
            await context.RespondAsync(result);
        }
        catch (DomainValidationException e)
        {
            await context.RespondAsync(e.ToDomainValidation());
        }
        
    }
}