using Health.Nurse.Domain.Console.Commands.Core;
using Health.Nurse.Domain.Storage.Sql.Core.Databases.NurseDb;
using Microsoft.Extensions.Logging;

namespace Health.Nurse.Domain.Console.Core.Decorators;

public sealed class TransactionCommandDecorator<TCommand, TOutput> : IAsyncCommandHandler<TCommand, TOutput>
    where TCommand : ICommand<TOutput>
{
    private readonly ILogger<TransactionCommandDecorator<TCommand, TOutput>> _logger;
    private readonly IAsyncCommandHandler<TCommand, TOutput> _handler;
    private readonly NurseDbContext _dbContext;

    public TransactionCommandDecorator(ILogger<TransactionCommandDecorator<TCommand, TOutput>> logger,IAsyncCommandHandler<TCommand, TOutput> handler, NurseDbContext dbContext)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _handler = handler ?? throw new ArgumentNullException(nameof(handler));
        _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
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