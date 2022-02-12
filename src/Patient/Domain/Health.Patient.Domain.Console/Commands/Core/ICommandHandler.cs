namespace Health.Patient.Domain.Console.Commands.Core;

public interface ICommandHandler<TCommand, TOutput> where TCommand : ICommand<TOutput>
{
    Task<TOutput> Handle(TCommand command);
}