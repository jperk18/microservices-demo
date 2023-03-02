using Health.Nurse.Domain.Console.Exceptions;
using Health.Nurse.Domain.Console.Services;
using Health.Nurse.Domain.Storage.Sql;
using Health.Shared.Domain.Exceptions;
using Health.Shared.Workflow.Processes.Commands;
using Health.Shared.Workflow.Processes.Events;
using MassTransit;

namespace Health.Nurse.Domain.Console.Consumer;

public class RegisterNurseConsumer : IConsumer<RegisterNurse>
{
    private readonly INurseValidationService<RegisterNurse> _validationService;
    private readonly INurseRepository _nurseRepository;

    public RegisterNurseConsumer(INurseValidationService<RegisterNurse> validationService, INurseRepository nurseRepository)
    {
        _validationService = validationService ?? throw new ArgumentNullException(nameof(validationService));
        _nurseRepository = nurseRepository ?? throw new ArgumentNullException(nameof(nurseRepository));
    }
    public async Task Consume(ConsumeContext<RegisterNurse> context)
    {
        try
        {
            await _validationService.Validate(context.Message);
            
            var p = await _nurseRepository.Nurses.Add(new Nurse.Domain.Storage.Sql.Core.Databases.NurseDb.Models.Nurse(
                Guid.NewGuid(), context.Message.FirstName, context.Message.LastName, context.Message.DateOfBirth
            ));
            
            await context.RespondAsync<RegisterNurseSuccess>(new
            {
                Id = p.Id,
                FirstName = p.FirstName,
                LastName = p.LastName,
                DateOfBirth = p.DateOfBirth
            });
            
            await context.Publish<NurseCreated>(new
            {
                Nurse = new
                {
                    Id = p.Id,
                    FirstName = p.FirstName,
                    LastName = p.LastName,
                    DateOfBirth = p.DateOfBirth
                }
            });
            
            await _nurseRepository.Complete();
        }
        catch (DomainValidationException e)
        {
            await context.RespondAsync<RegisterNurseFailed>(new
            {
                Error = e.ToWorkflowValidationObject()
            });
        }
    }
}