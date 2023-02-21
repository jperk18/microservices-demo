namespace Health.Shared.Domain.Mediator.Commands;

public interface IAsyncCommandHandler<TCommand, TOutput> where TCommand : ICommand<TOutput>
{
    Task<TOutput> Handle(TCommand command);
}