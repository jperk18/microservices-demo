using Health.Shared.Domain.Commands.Core;
using Health.Shared.Domain.Core.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Health.Shared.Domain.Core.Decorators;

public sealed class TransactionCommandDecorator<TCommand, TOutput> : IAsyncCommandHandler<TCommand, TOutput>
    where TCommand : ICommand<TOutput>
{
    private readonly ILogger<TransactionCommandDecorator<TCommand, TOutput>> _logger;
    private readonly IAsyncCommandHandler<TCommand, TOutput> _handler;
    private readonly IDbTransactionContextType _dbTransactionContextType;
    private readonly DbContext _dbContext;

    public TransactionCommandDecorator(ILogger<TransactionCommandDecorator<TCommand, TOutput>> logger,IAsyncCommandHandler<TCommand, TOutput> handler, IDbTransactionContextType dbTransactionContextType, IServiceProvider serviceProvider)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _handler = handler ?? throw new ArgumentNullException(nameof(handler));
        _dbTransactionContextType = dbTransactionContextType ?? throw new ArgumentNullException(nameof(dbTransactionContextType));
        _dbContext = (DbContext)serviceProvider.GetService(dbTransactionContextType.DatabaseContextType);
    }

    public async Task<TOutput> Handle(TCommand command)
    {
        await using var transaction = await _dbContext.Database.BeginTransactionAsync();
        try
        {
            var response = await _handler.Handle(command);
            await transaction.CommitAsync();
            return response;
        }
        catch (Exception)
        {
            await transaction.RollbackAsync();
            throw;
        }
    }
}

[AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = true)]
public sealed class TransactionPipelineAttribute : Attribute
{
    public TransactionPipelineAttribute()
    {
    }
}