﻿using Health.Shared.Domain.Commands.Core;
using Health.Shared.Domain.Queries.Core;

namespace Health.Shared.Domain.Mediator;

public sealed class Mediator : IMediator
{
    private readonly IServiceProvider _serviceProvider;

    public Mediator(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
    }
    
    public async Task<TResult> SendAsync<TResult>(ICommand<TResult> command)
    {
        var type = typeof(IAsyncCommandHandler<,>);
        var argTypes = new Type[] {command.GetType(), typeof(TResult)};
        var handlerType = type.MakeGenericType(argTypes);
        dynamic handler = _serviceProvider.GetService(handlerType) ?? throw new InvalidOperationException();
        TResult result = await handler.Handle((dynamic) command);
        return result;
    }
    
    public async Task<TResult> SendAsync<TResult>(IQuery<TResult> query)
    {
        var type = typeof(IAsyncQueryHandler<,>);
        var argTypes = new Type[] {query.GetType(), typeof(TResult)};
        var handlerType = type.MakeGenericType(argTypes);
        dynamic handler = _serviceProvider.GetService(handlerType) ?? throw new InvalidOperationException();
        TResult result = await handler.Handle((dynamic) query);
        return result;
    } 
}