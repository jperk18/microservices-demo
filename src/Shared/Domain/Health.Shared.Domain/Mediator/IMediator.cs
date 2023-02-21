using Health.Shared.Domain.Mediator.Commands;
using Health.Shared.Domain.Mediator.Queries;

namespace Health.Shared.Domain.Mediator;

public interface IMediator
{
    Task<TResult> SendAsync<TResult>(ICommand<TResult> command);
    Task<TResult> SendAsync<TResult>(IQuery<TResult> query);
}