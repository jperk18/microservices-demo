﻿namespace Health.Shared.Domain.Commands.Core;

public interface IAsyncCommandHandler<TCommand, TOutput> where TCommand : ICommand<TOutput>
{
    Task<TOutput> Handle(TCommand command);
}