using Health.Shared.Domain.Commands.Core;
using Health.Shared.Domain.Queries.Core;

namespace Health.Shared.Domain.Mediator;

public interface IMediator
{
    Task<TResult> SendAsync<TResult>(ICommand<TResult> command);
    Task<TResult> SendAsync<TResult>(IQuery<TResult> query);
}