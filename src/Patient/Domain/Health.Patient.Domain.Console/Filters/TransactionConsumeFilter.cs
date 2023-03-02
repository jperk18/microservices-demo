using Health.Patient.Domain.Storage.Sql;
using Health.Shared.Domain.Exceptions;
using MassTransit;

namespace Health.Patient.Domain.Console.Filters;

public class TransactionConsumeFilter<T> :
    IFilter<ConsumeContext<T>>
    where T : class
{
    private readonly IPatientRepository _dependency;
    public TransactionConsumeFilter(IPatientRepository dependency)
    {
        _dependency = dependency ?? throw new ArgumentNullException(nameof(dependency));
    }
      
    public async Task Send(ConsumeContext<T> context, IPipe<ConsumeContext<T>> next)
    {
        var transactionContext = context.GetPayload<TransactionContext>();
        try
        {
            await _dependency.EnlistTransaction(transactionContext.Transaction);
            await next.Send(context);
            await _dependency.Complete();
        }
        catch (DomainValidationException)
        {
            transactionContext.Rollback();
        }
        catch
        {
            transactionContext.Rollback();
            throw;
        }
    }
      
    public void Probe(ProbeContext context) { }
}