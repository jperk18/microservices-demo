using Health.Nurse.Domain.Console.Core.Exceptions;
using Health.Nurse.Domain.Console.Core.Models;
using Health.Nurse.Domain.Console.Queries.GetNurseQuery;
using Health.Shared.Domain.Mediator;
using MassTransit;

namespace Health.Nurse.Domain.Console.Activities;

public class NurseExistsActivity : IExecuteActivity<NurseRecordArguments>
{
    private readonly IMediator _mediator;

    public NurseExistsActivity(IMediator mediator)
    {
        _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
    }

    public async Task<ExecutionResult> Execute(ExecuteContext<NurseRecordArguments> context)
    {
        try
        {
            var record = await _mediator.SendAsync(new GetNurseQuery(context.Arguments.NurseId));
            return context.CompletedWithVariables<NurseRecordLog>(context, new {Nurse = record});
        }
        catch (NurseDomainValidationException e)
        {
            if(e.Errors?.FirstOrDefault()?.ErrorCode == "0001")
                return context.CompletedWithVariables<NurseRecordLog>(context, new {});
        }

        return context.Faulted();
    }
}

public interface NurseRecordArguments
{
    Guid NurseId { get; set; }
}

public interface NurseRecordLog
{
    NurseRecord? Nurse { get; set; }
}