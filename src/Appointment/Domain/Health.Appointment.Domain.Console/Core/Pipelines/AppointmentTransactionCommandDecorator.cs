using Health.Appointment.Domain.Storage.Sql.Appointment.Database;
using Health.Shared.Domain.Mediator.Commands;
using MassTransit.Transactions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Health.Appointment.Domain.Console.Core.Pipelines;

public class AppointmentTransactionCommandDecorator<TCommand, TOutput> : IAsyncCommandHandler<TCommand, TOutput>
    where TCommand : ICommand<TOutput>
{
    private readonly ILogger<AppointmentTransactionCommandDecorator<TCommand, TOutput>> _logger;
    private readonly IAsyncCommandHandler<TCommand, TOutput> _handler;
    private readonly ITransactionalBus _transactionalBus;
    private readonly DbContext _dbContext;

    public AppointmentTransactionCommandDecorator(ILogger<AppointmentTransactionCommandDecorator<TCommand, TOutput>> logger,IAsyncCommandHandler<TCommand, TOutput> handler, ITransactionalBus transactionalBus, AppointmentStateDbContext dbContext)
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
public sealed class AppointmentTransactionPipelineAttribute : Attribute
{
    public AppointmentTransactionPipelineAttribute()
    {
    }
}