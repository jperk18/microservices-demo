using Health.Patient.Domain.Console.Exceptions;
using Health.Patient.Domain.Console.Services;
using Health.Patient.Domain.Storage.Sql;
using Health.Shared.Domain.Exceptions;
using Health.Shared.Workflow.Processes.Commands;
using Health.Shared.Workflow.Processes.Events;
using MassTransit;

namespace Health.Patient.Domain.Console.Consumer;

public class RegisterPatientConsumer : IConsumer<RegisterPatient>
{
    private readonly IPatientValidationService<RegisterPatient> _validationService;
    private readonly IPatientRepository _patientRepository;

    public RegisterPatientConsumer(IPatientValidationService<RegisterPatient> validationService, IPatientRepository patientRepository)
    {
        _validationService = validationService ?? throw new ArgumentNullException(nameof(validationService));
        _patientRepository = patientRepository ?? throw new ArgumentNullException(nameof(patientRepository));
    }

    public async Task Consume(ConsumeContext<RegisterPatient> context)
    {
        try
        {
            await _validationService.Validate(context.Message);

            var patientId = NewId.NextGuid();
            var p = await _patientRepository.Patients.Add(new Storage.Sql.Databases.PatientDb.Models.Patient(
                patientId, context.Message.FirstName, context.Message.LastName, context.Message.DateOfBirth
            ));

            await context.Publish<PatientCreated>(new
            {
                Patient = new
                {
                    Id = p.Id,
                    FirstName = p.FirstName,
                    LastName = p.LastName,
                    DateOfBirth = p.DateOfBirth
                }
            });
            
            await context.RespondAsync<RegisterPatientSuccess>(new
            {
                p.Id,
                p.FirstName,
                p.LastName,
                p.DateOfBirth
            });
        }
        catch (DomainValidationException e)
        {
            await context.RespondAsync<RegisterPatientFailed>(new
            {
                Error = e.ToWorkflowValidationObject()
            });
        }
    }
}