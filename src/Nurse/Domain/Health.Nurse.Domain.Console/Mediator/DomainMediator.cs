using Health.Nurse.Domain.Console.Commands.Core;
using Health.Nurse.Domain.Console.Queries.Core;

namespace Health.Nurse.Domain.Console.Mediator;

public sealed class DomainMediator : IDomainMediator
{
    private readonly IServiceProvider _serviceProvider;

    public DomainMediator(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }
    
    public async Task<TResult> SendAsync<TResult>(ICommand<TResult> command)
    {
        var type = typeof(IAsyncCommandHandler<,>);
        var argTypes = new Type[] {command.GetType(), typeof(TResult)};
        var handlerType = type.MakeGenericType(argTypes);
        dynamic handler = _serviceProvider.GetService(handlerType);
        TResult result = await handler.Handle((dynamic) command);
        return result;
    }
    
    public async Task<TResult> SendAsync<TResult>(IQuery<TResult> query)
    {
        var type = typeof(IAsyncQueryHandler<,>);
        var argTypes = new Type[] {query.GetType(), typeof(TResult)};
        var handlerType = type.MakeGenericType(argTypes);
        dynamic handler = _serviceProvider.GetService(handlerType);
        TResult result = await handler.Handle((dynamic) query);
        return result;
    } 
}