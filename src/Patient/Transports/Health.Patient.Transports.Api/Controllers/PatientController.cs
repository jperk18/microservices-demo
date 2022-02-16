using System.Net.Mime;
using Health.Patient.Transports.Api.Middleware;
using Health.Patient.Transports.Api.Models;
using Health.Workflow.Shared.Processes;
using Health.Workflow.Shared.Processes.Core.Exceptions.Models;
using MassTransit;
using Microsoft.AspNetCore.Mvc;

namespace Health.Patient.Transports.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PatientController : ControllerBase
{
    private readonly ILogger<PatientController> _logger;

    private readonly IRequestClient<Workflow.Shared.Processes.RegisterPatientCommandQuery>
        _registerPatientRequestClient;

    private readonly IRequestClient<Workflow.Shared.Processes.GetAllPatientsQuery> _getAllPatientsRequestClient;
    private readonly IRequestClient<Workflow.Shared.Processes.GetPatientQuery> _getPatientRequestClient;
    private readonly IRequestClient<Workflow.Shared.Processes.CheckInPatientCommandQuery> _checkInPatientRequestClient;

    public PatientController(ILogger<PatientController> logger,
        IRequestClient<Workflow.Shared.Processes.RegisterPatientCommandQuery> registerPatientRequestClient,
        IRequestClient<Workflow.Shared.Processes.GetAllPatientsQuery> getAllPatientsRequestClient,
        IRequestClient<Workflow.Shared.Processes.GetPatientQuery> getPatientRequestClient,
        IRequestClient<Workflow.Shared.Processes.CheckInPatientCommandQuery> checkInPatientRequestClient
    )
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _registerPatientRequestClient = registerPatientRequestClient ??
                                        throw new ArgumentNullException(nameof(registerPatientRequestClient));
        _getAllPatientsRequestClient = getAllPatientsRequestClient ??
                                       throw new ArgumentNullException(nameof(getAllPatientsRequestClient));
        _getPatientRequestClient =
            getPatientRequestClient ?? throw new ArgumentNullException(nameof(getPatientRequestClient));
        _checkInPatientRequestClient = checkInPatientRequestClient ?? throw new ArgumentNullException(nameof(checkInPatientRequestClient));
    }

    [HttpPost()]
    [Consumes(MediaTypeNames.Application.Json)]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(CreatePatientApiResponse))]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity, Type = typeof(ApiGenericValidationResultObject))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Register([FromBody] CreatePatientApiRequest request)
    {
        var (result, errors) = await _registerPatientRequestClient
            .GetResponse<Workflow.Shared.Processes.Core.Models.Patient, WorkflowValidation>(
                new RegisterPatientCommandQuery()
                    {FirstName = request.FirstName, LastName = request.LastName, DateOfBirth = request.DateOfBirth});

        if (result.IsCompletedSuccessfully)
        {
            var response = await result;
            return new ObjectResult(new CreatePatientApiResponse() {PatientId = response.Message.PatientId})
                {StatusCode = StatusCodes.Status201Created};
        }

        var domainError = await errors;
        throw domainError.Message.ToException();
    }

    [HttpPost("CheckIn")]
    [Consumes(MediaTypeNames.Application.Json)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity, Type = typeof(ApiGenericValidationResultObject))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> CheckIn([FromQuery] Guid patientId)
    {
        //Check-in patient
        var (result, errors) = await _checkInPatientRequestClient
            .GetResponse<CheckInPatientSuccessResponse, CheckInPatientFailResponse>(new CheckInPatientCommandQuery(patientId));

        if (result.IsCompletedSuccessfully)
        {
            return Ok();
        }

        var domainError = await errors;
        throw domainError.Message.Error.ToException();
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GetPatientApiResponse))]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity, Type = typeof(ApiGenericValidationResultObject))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetPatient([FromQuery] GetPatientApiRequest request)
    {
        var (result, errors) = await _getPatientRequestClient
            .GetResponse<Workflow.Shared.Processes.Core.Models.Patient, WorkflowValidation>(
                new GetPatientQuery() {Id = request.PatientId});

        if (result.IsCompletedSuccessfully)
        {
            var response = await result;
            return Ok(new GetPatientApiResponse(response.Message.PatientId, response.Message.FirstName,
                response.Message.LastName, response.Message.DateOfBirth));
        }

        var domainError = await errors;
        throw domainError.Message.ToException();
    }

    [HttpGet("All")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<GetPatientApiResponse>))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetAllPatients()
    {
        var (result, errors) = await _getAllPatientsRequestClient
            .GetResponse<Workflow.Shared.Processes.Core.Models.Patient[], WorkflowValidation>(
                new GetAllPatientsQuery());

        if (result.IsCompletedSuccessfully)
        {
            var response = await result;
            return Ok(response.Message.Select(res =>
                new GetPatientApiResponse(res.PatientId, res.FirstName, res.LastName, res.DateOfBirth)));
        }

        var domainError = await errors;
        throw domainError.Message.ToException();
    }
}