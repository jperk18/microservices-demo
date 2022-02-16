namespace Health.Shared.Domain.Queries.Core;

public interface IAsyncQueryHandler<TQuery, TResult> where TQuery : IQuery<TResult>
{
    Task<TResult> Handle(TQuery command);
}