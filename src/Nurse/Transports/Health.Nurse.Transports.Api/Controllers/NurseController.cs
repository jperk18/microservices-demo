using System.Net.Mime;
using Health.Nurse.Transports.Api.Middleware;
using Health.Nurse.Transports.Api.Models;
using Health.Shared.Workflow.Processes;
using Health.Shared.Workflow.Processes.Commands;
using Health.Shared.Workflow.Processes.Core.Exceptions;
using Health.Shared.Workflow.Processes.Queries;
using Health.Shared.Workflow.Processes.Sagas.Appointment;
using MassTransit;
using Microsoft.AspNetCore.Mvc;

namespace Health.Nurse.Transports.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class NurseController : ControllerBase
{
    private readonly ILogger<NurseController> _logger;
    private readonly IRequestClient<RegisterNurse> _registerNurseRequestClient;
    private readonly IRequestClient<GetAllNurses> _getAllNursesRequestClient;
    private readonly IRequestClient<GetNurse> _getNurseRequestClient;
    private readonly IRequestClient<GetWaitingPatientsForNurses> _getWaitingPatients;

    public NurseController(ILogger<NurseController> logger,
        IRequestClient<RegisterNurse> registerNurseRequestClient,
        IRequestClient<GetAllNurses> getAllNursesRequestClient,
        IRequestClient<GetNurse> getNurseRequestClient,
        IRequestClient<GetWaitingPatientsForNurses> getWaitingPatients)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _registerNurseRequestClient = registerNurseRequestClient ??
                                      throw new ArgumentNullException(nameof(registerNurseRequestClient));
        _getAllNursesRequestClient = getAllNursesRequestClient ??
                                     throw new ArgumentNullException(nameof(getAllNursesRequestClient));
        _getNurseRequestClient =
            getNurseRequestClient ?? throw new ArgumentNullException(nameof(getNurseRequestClient));
        _getWaitingPatients = getWaitingPatients ?? throw new ArgumentNullException(nameof(getWaitingPatients));
    }

    [HttpPost()]
    [Consumes(MediaTypeNames.Application.Json)]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(CreateNurseApiResponse))]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity, Type = typeof(NurseApiGenericValidationResultObject))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Register([FromBody] CreateNurseApiRequest request)
    {
        var (result, errors) = await _registerNurseRequestClient
            .GetResponse<RegisterNurseSuccess, RegisterNurseFailed>(new
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                DateOfBirth = request.DateOfBirth
            });

        if (result.IsCompletedSuccessfully)
        {
            var response = await result;
            return new ObjectResult(new CreateNurseApiResponse() {Id = response.Message.Id})
                {StatusCode = StatusCodes.Status201Created};
        }

        var domainError = await errors;
        throw new WorkflowValidationException(domainError.Message.Error);
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GetNurseApiResponse))]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity, Type = typeof(NurseApiGenericValidationResultObject))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetNurse([FromQuery] GetNurseApiRequest request)
    {
        var (result, errors) = await _getNurseRequestClient
            .GetResponse<GetNurseSuccess, GetNurseFailed>(new
            {
                Id = request.Id
            });

        if (result.IsCompletedSuccessfully)
        {
            var response = await result;
            return Ok(new GetNurseApiResponse(response.Message.Nurse.Id, response.Message.Nurse.FirstName,
                response.Message.Nurse.LastName));
        }

        var domainError = await errors;
        throw new WorkflowValidationException(domainError.Message.Error);
    }

    [HttpGet("All")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<GetNurseApiResponse>))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetAllNurses()
    {
        var response = await _getAllNursesRequestClient.GetResponse<GetAllNursesSuccess>(new { });
        return Ok(response.Message.Nurses.Select(res => new GetNurseApiResponse(res.Id, res.FirstName, res.LastName)));
    }
}