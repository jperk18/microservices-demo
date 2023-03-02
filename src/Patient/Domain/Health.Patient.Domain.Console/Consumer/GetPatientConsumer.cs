using Health.Patient.Domain.Console.Exceptions;
using Health.Patient.Domain.Console.Services;
using Health.Patient.Domain.Storage.Sql;
using Health.Shared.Domain.Exceptions;
using Health.Shared.Workflow.Processes.Queries;
using MassTransit;

namespace Health.Patient.Domain.Console.Consumer;

public class GetPatientConsumer : IConsumer<GetPatient>
{
    private readonly IPatientValidationService<GetPatient> _validationService;
    private readonly IPatientRepository _patientRepository;

    public GetPatientConsumer(IPatientValidationService<GetPatient> validationService, IPatientRepository patientRepository)
    {
        _validationService = validationService ?? throw new ArgumentNullException(nameof(validationService));
        _patientRepository = patientRepository ?? throw new ArgumentNullException(nameof(patientRepository));
    }
    public async Task Consume(ConsumeContext<GetPatient> context)
    {
        try
        {
            await _validationService.Validate(context.Message);
            
            var result = await _patientRepository.Patients.GetById(context.Message.Id);

            if (result == null)
                throw new PatientDomainValidationException($"Patient does not exist for {context.Message.Id}");
        
            await context.RespondAsync<GetPatientSuccess>(new
            {
                Patient = new
                {
                    result.Id,
                    result.FirstName,
                    result.LastName,
                    result.DateOfBirth
                }
            });
        }
        catch (DomainValidationException e)
        {
            await context.RespondAsync<GetPatientFailed>(new
            {
                Error = e.ToWorkflowValidationObject()
            });
        }
    }
}