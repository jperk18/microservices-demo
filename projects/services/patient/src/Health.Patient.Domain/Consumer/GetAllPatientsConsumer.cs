using Health.Patient.Domain.Commands.Core;
using Health.Patient.Domain.Commands.CreatePatientCommand;
using Health.Patient.Domain.Core.Exceptions;
using Health.Patient.Domain.Core.Models;
using Health.Patient.Domain.Queries.Core;
using Health.Patient.Domain.Queries.GetAllPatientsQuery;
using MassTransit;

namespace Health.Patient.Domain.Consumer;

public class GetAllPatientsConsumer : IConsumer<GetAllPatientsQuery>
{
    private readonly IQueryHandler<GetAllPatientsQuery, IEnumerable<PatientRecord>> _cmdHandler;

    public GetAllPatientsConsumer(IQueryHandler<GetAllPatientsQuery, IEnumerable<PatientRecord>> cmdHandler)
    {
        _cmdHandler = cmdHandler ?? throw new ArgumentNullException(nameof(cmdHandler));
    }
    public async Task Consume(ConsumeContext<GetAllPatientsQuery> context)
    {
        try
        {
            var result = await _cmdHandler.Handle(context.Message);
            await context.RespondAsync(result.ToArray());
        }
        catch (DomainValidationException e)
        {
            await context.RespondAsync(e.ToDomainValidation());
        }
        
    }
}