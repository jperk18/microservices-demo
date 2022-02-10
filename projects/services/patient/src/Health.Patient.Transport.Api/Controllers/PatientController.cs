using System.Net.Mime;
using Health.Patient.Api.Requests;
using Health.Patient.Domain.Commands.Core;
using Health.Patient.Domain.Commands.CreatePatientCommand;
using Health.Patient.Domain.Core.Exceptions.Models;
using Health.Patient.Domain.Core.Models;
using Health.Patient.Domain.Queries.Core;
using Health.Patient.Domain.Queries.GetAllPatientsQuery;
using Health.Patient.Domain.Queries.GetPatientQuery;
using Health.Patient.Transport.Api.Middleware;
using MassTransit;
using Microsoft.AspNetCore.Mvc;

namespace Health.Patient.Transport.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PatientController : ControllerBase
{
    private readonly ILogger<PatientController> _logger;
    private readonly IQueryHandler<GetPatientQuery, PatientRecord> _getPatientHandler;
    private readonly IQueryHandler<GetAllPatientsQuery, IEnumerable<PatientRecord>> _getAllPatientsHandler;
    private readonly IRequestClient<CreatePatientCommand> _createPatientRequestClient;

    public PatientController(ILogger<PatientController> logger,
        ICommandHandler<CreatePatientCommand, PatientRecord> createPatientHandler,
        IQueryHandler<GetPatientQuery, PatientRecord> getPatientHandler,
        IQueryHandler<GetAllPatientsQuery, IEnumerable<PatientRecord>> getAllPatientsHandler,
        IRequestClient<CreatePatientCommand> createPatientRequestClient)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _getPatientHandler = getPatientHandler ?? throw new ArgumentNullException(nameof(getPatientHandler));
        _getAllPatientsHandler =
            getAllPatientsHandler ?? throw new ArgumentNullException(nameof(getAllPatientsHandler));
        _createPatientRequestClient = createPatientRequestClient ??
                                      throw new ArgumentNullException(nameof(createPatientRequestClient));
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
        var response = await _getPatientHandler.Handle(new GetPatientQuery() {PatientId = request.PatientId});
        return Ok(new GetPatientApiResponse(response.Id, response.FirstName, response.LastName, response.DateOfBirth));
    }

    [HttpGet("All")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<GetPatientApiResponse>))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetAllPatients()
    {
        var r = (await _getAllPatientsHandler.Handle(new GetAllPatientsQuery()))
            .Select(response =>
                new GetPatientApiResponse(response.Id, response.FirstName, response.LastName, response.DateOfBirth));
        return Ok(r);
    }
}