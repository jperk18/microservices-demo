using Health.Patient.Domain.Console.Core.Exceptions;
using Health.Patient.Domain.Console.Core.Exceptions.Helpers;
using Health.Patient.Domain.Console.Core.Models;
using Health.Patient.Domain.Console.Queries.Core;
using Health.Patient.Domain.Console.Queries.GetPatientQuery;
using MassTransit;

namespace Health.Patient.Domain.Console.Consumer;

public class GetPatientConsumer : IConsumer<Workflow.Shared.Processes.GetPatientQuery>
{
    private readonly IQueryHandler<GetPatientQuery, PatientRecord> _cmdHandler;

    public GetPatientConsumer(IQueryHandler<GetPatientQuery, PatientRecord> cmdHandler)
    {
        _cmdHandler = cmdHandler ?? throw new ArgumentNullException(nameof(cmdHandler));
    }
    public async Task Consume(ConsumeContext<Workflow.Shared.Processes.GetPatientQuery> context)
    {
        try
        {
            var result = await _cmdHandler.Handle(new GetPatientQuery(context.Message.PatientId));
            await context.RespondAsync(new Workflow.Shared.Processes.Core.Models.Patient()
            {
                DateOfBirth = result.DateOfBirth, FirstName = result.FirstName, LastName = result.LastName, PatientId = result.Id
            });
        }
        catch (DomainValidationException e)
        {
            await context.RespondAsync(e.ToValidationObject().ToWorkflowValidationObject());
        }
        
    }
}