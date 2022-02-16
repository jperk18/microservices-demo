using Health.Nurse.Domain.Console.Commands.Core;
using Health.Nurse.Domain.Console.Queries.Core;

namespace Health.Nurse.Domain.Console.Mediator;

public interface IDomainMediator
{
    Task<TResult> SendAsync<TResult>(ICommand<TResult> command);
    Task<TResult> SendAsync<TResult>(IQuery<TResult> query);
}