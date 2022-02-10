using System.Net.Mime;
using Health.Patient.Api.Requests;
using Health.Patient.Domain.Commands.CreatePatientCommand;
using Health.Patient.Domain.Core.Exceptions.Models;
using Health.Patient.Domain.Core.Models;
using Health.Patient.Domain.Queries.Core;
using Health.Patient.Transport.Api.Middleware;
using Health.Workflow.Processes;
using MassTransit;
using Microsoft.AspNetCore.Mvc;
using GetAllPatientsQuery = Health.Patient.Domain.Queries.GetAllPatientsQuery.GetAllPatientsQuery;
using GetPatientQuery = Health.Patient.Domain.Queries.GetPatientQuery.GetPatientQuery;

namespace Health.Patient.Transport.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PatientController : ControllerBase
{
    private readonly ILogger<PatientController> _logger;
    private readonly IRequestClient<CreatePatientCommand> _createPatientRequestClient;
    private readonly IRequestClient<GetAllPatientsQuery> _getAllPatientsRequestClient;
    private readonly IRequestClient<GetPatientQuery> _getPatientRequestClient;
    private readonly IRequestClient<RegisterPatientCommandQuery> _registerPatientRequestClient;

    public PatientController(ILogger<PatientController> logger,
        IRequestClient<CreatePatientCommand> createPatientRequestClient,
        IRequestClient<GetAllPatientsQuery> getAllPatientsRequestClient,
        IRequestClient<GetPatientQuery> getPatientRequestClient
        )
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _createPatientRequestClient = createPatientRequestClient ??
                                      throw new ArgumentNullException(nameof(createPatientRequestClient));
        _getAllPatientsRequestClient = getAllPatientsRequestClient ?? throw new ArgumentNullException(nameof(getAllPatientsRequestClient));
        _getPatientRequestClient = getPatientRequestClient ?? throw new ArgumentNullException(nameof(getPatientRequestClient));
    }

    [HttpPost()]
    [Consumes(MediaTypeNames.Application.Json)]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(CreatePatientApiResponse))]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity, Type = typeof(ApiGenericException))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Register([FromBody] CreatePatientApiRequest request)
    {
        var (result, errors) = await _createPatientRequestClient.GetResponse<PatientRecord, DomainValidation>(
            new CreatePatientCommand(
                request.FirstName, request.LastName,
                request.DateOfBirth));

        if (result.IsCompletedSuccessfully)
        {
            var response = await result;
            return new ObjectResult(new CreatePatientApiResponse() {PatientId = response.Message.Id})
                {StatusCode = StatusCodes.Status201Created};
        }

        var domainError = await errors;
        throw domainError.Message.ToDomainValidationException();
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GetPatientApiResponse))]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity, Type = typeof(ApiGenericException))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetPatient([FromQuery] GetPatientApiRequest request)
    {
        var (result, errors) = await _getPatientRequestClient.GetResponse<PatientRecord, DomainValidation>(
            new GetPatientQuery(){ PatientId = request.PatientId});
        
        if (result.IsCompletedSuccessfully)
        {
            var response = await result;
            return Ok(new GetPatientApiResponse(response.Message.Id, response.Message.FirstName, response.Message.LastName, response.Message.DateOfBirth));
        }

        var domainError = await errors;
        throw domainError.Message.ToDomainValidationException();
    }

    [HttpGet("All")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<GetPatientApiResponse>))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetAllPatients()
    {
        var (result, errors) = await _getAllPatientsRequestClient.GetResponse<PatientRecord[], DomainValidation>(
            new GetAllPatientsQuery());
        
        if (result.IsCompletedSuccessfully)
        {
            var response = await result;
            return Ok(response.Message.Select(res => new GetPatientApiResponse(res.Id, res.FirstName, res.LastName, res.DateOfBirth)));
        }

        var domainError = await errors;
        throw domainError.Message.ToDomainValidationException();
    }
}