using Health.Patient.Domain.Console.Core.Exceptions;
using Health.Patient.Domain.Console.Core.Exceptions.Helpers;
using Health.Patient.Domain.Console.Core.Models;
using Health.Patient.Domain.Console.Mediator;
using Health.Patient.Domain.Console.Queries.Core;
using Health.Patient.Domain.Console.Queries.GetAllPatientsQuery;
using MassTransit;

namespace Health.Patient.Domain.Console.Consumer;

public class GetAllPatientsConsumer : IConsumer<Workflow.Shared.Processes.GetAllPatientsQuery>
{
    private readonly IDomainMediator _mediator;

    public GetAllPatientsConsumer(IDomainMediator mediator)
    {
        _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
    }
    public async Task Consume(ConsumeContext<Workflow.Shared.Processes.GetAllPatientsQuery> context)
    {
        try
        {
            var r = await _mediator.SendAsync(new GetAllPatientsQuery());
            await context.RespondAsync(r.Select(result => new Workflow.Shared.Processes.Core.Models.Patient()
            {
                DateOfBirth = result.DateOfBirth, FirstName = result.FirstName, LastName = result.LastName, PatientId = result.Id
            }).ToArray());
        }
        catch (DomainValidationException e)
        {
            await context.RespondAsync(e.ToValidationObject().ToWorkflowValidationObject());
        }
        
    }
}