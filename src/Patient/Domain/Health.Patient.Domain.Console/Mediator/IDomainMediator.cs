using Health.Patient.Domain.Console.Commands.Core;
using Health.Patient.Domain.Console.Queries.Core;

namespace Health.Patient.Domain.Console.Mediator;

public interface IDomainMediator
{
    Task<TResult> SendAsync<TResult>(ICommand<TResult> command);
    Task<TResult> SendAsync<TResult>(IQuery<TResult> query);
}