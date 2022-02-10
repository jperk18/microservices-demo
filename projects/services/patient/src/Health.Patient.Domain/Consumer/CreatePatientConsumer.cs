using Health.Patient.Domain.Commands.Core;
using Health.Patient.Domain.Commands.CreatePatientCommand;
using Health.Patient.Domain.Core.Exceptions;
using Health.Patient.Domain.Core.Models;
using MassTransit;

namespace Health.Patient.Domain.Consumer;

public class CreatePatientCommandConsumer : IConsumer<CreatePatientCommand>
{
    private readonly ICommandHandler<CreatePatientCommand, PatientRecord> _cmdHandler;

    public CreatePatientCommandConsumer(ICommandHandler<CreatePatientCommand, PatientRecord> cmdHandler)
    {
        _cmdHandler = cmdHandler ?? throw new ArgumentNullException(nameof(cmdHandler));
    }
    public async Task Consume(ConsumeContext<CreatePatientCommand> context)
    {
        try
        {
            var result = await _cmdHandler.Handle(context.Message);
            await context.RespondAsync<PatientRecord>(result);
        }
        catch (DomainValidationException e)
        {
            await context.RespondAsync(e.ToDomainValidation());
        }
        
    }
}