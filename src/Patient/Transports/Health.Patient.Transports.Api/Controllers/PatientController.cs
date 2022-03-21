using System.Net.Mime;
using Health.Patient.Transports.Api.Middleware;
using Health.Patient.Transports.Api.Models;
using Health.Shared.Workflow.Processes.Commands;
using Health.Shared.Workflow.Processes.Core.Exceptions;
using Health.Shared.Workflow.Processes.Queries;
using MassTransit;
using Microsoft.AspNetCore.Mvc;

namespace Health.Patient.Transports.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PatientController : ControllerBase
{
    private readonly ILogger<PatientController> _logger;

    private readonly IRequestClient<RegisterPatient>
        _registerPatientRequestClient;

    private readonly IRequestClient<GetAllPatients> _getAllPatientsRequestClient;
    private readonly IRequestClient<GetPatient> _getPatientRequestClient;

    public PatientController(ILogger<PatientController> logger,
        IRequestClient<RegisterPatient> registerPatientRequestClient,
        IRequestClient<GetAllPatients> getAllPatientsRequestClient,
        IRequestClient<GetPatient> getPatientRequestClient
    )
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _registerPatientRequestClient = registerPatientRequestClient ??
                                        throw new ArgumentNullException(nameof(registerPatientRequestClient));
        _getAllPatientsRequestClient = getAllPatientsRequestClient ??
                                       throw new ArgumentNullException(nameof(getAllPatientsRequestClient));
        _getPatientRequestClient =
            getPatientRequestClient ?? throw new ArgumentNullException(nameof(getPatientRequestClient));
    }

    [HttpPost()]
    [Consumes(MediaTypeNames.Application.Json)]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(CreatePatientApiResponse))]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity, Type = typeof(PatientApiGenericValidationResultObject))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Register([FromBody] CreatePatientApiRequest request)
    {
        var (result, errors) = await _registerPatientRequestClient
            .GetResponse<RegisterPatientSuccess, RegisterPatientFailed>(new
            {
                request.FirstName,
                request.LastName,
                request.DateOfBirth
            });

        if (result.IsCompletedSuccessfully)
        {
            var response = await result;
            return new ObjectResult(new CreatePatientApiResponse() {PatientId = response.Message.Id})
                {StatusCode = StatusCodes.Status201Created};
        }

        var domainError = await errors;
        throw new WorkflowValidationException(domainError.Message.Error);
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GetPatientApiResponse))]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity, Type = typeof(PatientApiGenericValidationResultObject))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetPatient([FromQuery] GetPatientApiRequest request)
    {
        var (result, errors) = await _getPatientRequestClient
            .GetResponse<GetPatientSuccess, GetPatientFailed>(new
            {
                Id = request.PatientId
            });

        if (result.IsCompletedSuccessfully)
        {
            var response = await result;
            return Ok(new GetPatientApiResponse(response.Message.Patient.Id, response.Message.Patient.FirstName,
                response.Message.Patient.LastName, response.Message.Patient.DateOfBirth));
        }

        var domainError = await errors;
        throw new WorkflowValidationException(domainError.Message.Error);
    }

    [HttpGet("All")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<GetPatientApiResponse>))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetAllPatients()
    {
        var response = await _getAllPatientsRequestClient
            .GetResponse<GetAllPatientsSuccess>(new { });

        return Ok(response.Message.Patients.Select(res =>
            new GetPatientApiResponse(res.Id, res.FirstName, res.LastName, res.DateOfBirth)));
    }
}