using Health.Patient.Domain.Console.Commands.Core;
using Health.Patient.Domain.Console.Commands.CreatePatientCommand;
using Health.Patient.Domain.Console.Core.Exceptions;
using Health.Patient.Domain.Console.Core.Exceptions.Helpers;
using Health.Patient.Domain.Console.Core.Models;
using Health.Workflow.Shared.Processes;
using MassTransit;

namespace Health.Patient.Domain.Console.Consumer;

public class RegisterPatientCommandQueryConsumer : IConsumer<RegisterPatientCommandQuery>
{
    private readonly ICommandHandler<CreatePatientCommand, PatientRecord> _cmdHandler;

    public RegisterPatientCommandQueryConsumer(ICommandHandler<CreatePatientCommand, PatientRecord> cmdHandler)
    {
        _cmdHandler = cmdHandler ?? throw new ArgumentNullException(nameof(cmdHandler));
    }
    public async Task Consume(ConsumeContext<RegisterPatientCommandQuery> context)
    {
        try
        {
            var result =
                await _cmdHandler.Handle(new CreatePatientCommand(context.Message.FirstName, context.Message.LastName, context.Message.DateOfBirth));
            await context.RespondAsync(new Workflow.Shared.Processes.Core.Models.Patient()
            {
                DateOfBirth = result.DateOfBirth, FirstName = result.FirstName, LastName = result.LastName, PatientId = result.Id
            });
        }
        catch (DomainValidationException e)
        {
            await context.RespondAsync(e.ToValidationObject().ToWorkflowValidationObject());
        }
        
    }
}