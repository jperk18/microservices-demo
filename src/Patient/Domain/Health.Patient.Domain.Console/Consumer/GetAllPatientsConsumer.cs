using Health.Patient.Domain.Storage.Sql;
using Health.Shared.Workflow.Processes.Queries;
using MassTransit;

namespace Health.Patient.Domain.Console.Consumer;

public class GetAllPatientsConsumer : IConsumer<GetAllPatients>
{
    private readonly IPatientRepository _patientRepository;

    public GetAllPatientsConsumer(IPatientRepository patientRepository)
    {
        _patientRepository = patientRepository ?? throw new ArgumentNullException(nameof(patientRepository));
    }

    public async Task Consume(ConsumeContext<GetAllPatients> context)
    {
        var r = _patientRepository.Patients.GetAll();
        var patientRecords = r as Storage.Sql.Databases.PatientDb.Models.Patient[] ?? r.ToArray();

        await context.RespondAsync<GetAllPatientsSuccess>(new
        {
            Patients = patientRecords.Select(result => new Shared.Workflow.Processes.Inner.Models.PatientDto(
                result.Id, result.FirstName, result.LastName, result.DateOfBirth
            )).ToArray()
        });
    }
}