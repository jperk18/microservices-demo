using Health.Nurse.Domain.Console.Exceptions;
using Health.Nurse.Domain.Storage.Sql;
using Health.Shared.Domain.Exceptions;
using Health.Shared.Domain.Exceptions.Models;
using Health.Shared.Domain.Services;
using Health.Shared.Workflow.Processes.Queries;
using MassTransit;

namespace Health.Nurse.Domain.Console.Consumer;

public class GetNurseConsumer : IConsumer<GetNurse>
{
    private readonly IValidationService<GetNurse> _validationService;
    private readonly INurseRepository _nurseRepository;


    public GetNurseConsumer(IValidationService<GetNurse> validationService, INurseRepository nurseRepository)
    {
        _nurseRepository = nurseRepository ?? throw new ArgumentNullException(nameof(nurseRepository));
        _validationService = validationService ?? throw new ArgumentNullException(nameof(validationService));
    }

    public async Task Consume(ConsumeContext<GetNurse> context)
    {
        try
        {
            await _validationService.Validate(context.Message);
            
            var result = await _nurseRepository.Nurses.GetById(context.Message.Id);

            if (result == null)
                throw new NurseDomainValidationException($"Record does not exist for {context.Message.Id}", new DomainValidationFailureDto[]{new("0001", "Nurse does not exist")});

            
            await context.RespondAsync<GetNurseSuccess>(new
            {
                Nurse = new
                {
                    Id = result.Id,
                    FirstName = result.FirstName,
                    LastName = result.LastName,
                    DateOfBirth = result.DateOfBirth
                }
            });
        }
        catch (DomainValidationException e)
        {
            await context.RespondAsync<GetNurseFailed>(new
            {
                Error = e.ToWorkflowValidationObject()
            });
        }
    }
}