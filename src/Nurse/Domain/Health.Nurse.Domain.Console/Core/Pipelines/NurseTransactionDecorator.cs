using Health.Nurse.Domain.Storage.Sql.Core.Databases.NurseDb;
using Health.Shared.Domain.Mediator.Commands;
using MassTransit.Transactions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Health.Nurse.Domain.Console.Core.Pipelines;

public class NurseTransactionCommandDecorator<TCommand, TOutput> : IAsyncCommandHandler<TCommand, TOutput>
    where TCommand : ICommand<TOutput>
{
    private readonly ILogger<NurseTransactionCommandDecorator<TCommand, TOutput>> _logger;
    private readonly IAsyncCommandHandler<TCommand, TOutput> _handler;
    private readonly ITransactionalBus _transactionalBus;
    private readonly DbContext _dbContext;

    public NurseTransactionCommandDecorator(ILogger<NurseTransactionCommandDecorator<TCommand, TOutput>> logger,IAsyncCommandHandler<TCommand, TOutput> handler, ITransactionalBus transactionalBus, NurseDbContext dbContext)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _handler = handler ?? throw new ArgumentNullException(nameof(handler));
        _transactionalBus = transactionalBus ?? throw new ArgumentNullException(nameof(transactionalBus));
        _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
    }

    public async Task<TOutput> Handle(TCommand command)
    {
        await using var transaction = await _dbContext.Database.BeginTransactionAsync();
        try
        {
            var response = await _handler.Handle(command);
            await transaction.CommitAsync();
            await _transactionalBus.Release();
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
public sealed class NurseTransactionPipelineAttribute : Attribute
{
    public NurseTransactionPipelineAttribute()
    {
    }
}