using System.Net.Mime;
using Health.Nurse.Transports.Api.Middleware;
using Health.Nurse.Transports.Api.Models;
using Health.Workflow.Shared.Processes;
using Health.Workflow.Shared.Processes.Core.Exceptions.Models;
using MassTransit;
using Microsoft.AspNetCore.Mvc;

namespace Health.Nurse.Transports.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class NurseController : ControllerBase
{
    private readonly ILogger<NurseController> _logger;
    private readonly IRequestClient<RegisterNurseCommandQuery> _registerNurseRequestClient;
    private readonly IRequestClient<GetAllNursesQuery> _getAllNursesRequestClient;
    private readonly IRequestClient<GetNurseQuery> _getNurseRequestClient;
    private readonly IRequestClient<GetWaitingPatientsForNurses> _getWaitingPatients;

    public NurseController(ILogger<NurseController> logger,
        IRequestClient<RegisterNurseCommandQuery> registerNurseRequestClient,
        IRequestClient<GetAllNursesQuery> getAllNursesRequestClient,
        IRequestClient<GetNurseQuery> getNurseRequestClient,
        IRequestClient<GetWaitingPatientsForNurses> getWaitingPatients )
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _registerNurseRequestClient = registerNurseRequestClient ??
                                        throw new ArgumentNullException(nameof(registerNurseRequestClient));
        _getAllNursesRequestClient = getAllNursesRequestClient ?? throw new ArgumentNullException(nameof(getAllNursesRequestClient));
        _getNurseRequestClient = getNurseRequestClient ?? throw new ArgumentNullException(nameof(getNurseRequestClient));
        _getWaitingPatients = getWaitingPatients ?? throw new ArgumentNullException(nameof(getWaitingPatients));
    }

    [HttpPost()]
    [Consumes(MediaTypeNames.Application.Json)]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(CreateNurseApiResponse))]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity, Type = typeof(ApiGenericValidationResultObject))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Register([FromBody] CreateNurseApiRequest request)
    {
        var (result, errors) = await _registerNurseRequestClient.GetResponse<Workflow.Shared.Processes.Core.Models.Nurse, WorkflowValidation>(
            new RegisterNurseCommandQuery(){FirstName = request.FirstName, LastName = request.LastName, DateOfBirth = request.DateOfBirth });

        if (result.IsCompletedSuccessfully)
        {
            var response = await result;
            return new ObjectResult(new CreateNurseApiResponse() {Id = response.Message.Id})
                {StatusCode = StatusCodes.Status201Created};
        }

        var domainError = await errors;
        throw domainError.Message.ToException();
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GetNurseApiResponse))]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity, Type = typeof(ApiGenericValidationResultObject))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetNurse([FromQuery] GetNurseApiRequest request)
    {
        var (result, errors) = await _getNurseRequestClient.GetResponse<Workflow.Shared.Processes.Core.Models.Nurse, WorkflowValidation>(
            new GetNurseQuery(){ Id = request.Id});
        
        if (result.IsCompletedSuccessfully)
        {
            var response = await result;
            return Ok(new GetNurseApiResponse(response.Message.Id, response.Message.FirstName, response.Message.LastName));
        }

        var domainError = await errors;
        throw domainError.Message.ToException();
    }

    [HttpGet("All")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<GetNurseApiResponse>))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetAllNurses()
    {
        var (result, errors) = await _getAllNursesRequestClient.GetResponse<Workflow.Shared.Processes.Core.Models.Nurse[], WorkflowValidation>(
            new GetAllNursesQuery());
        
        if (result.IsCompletedSuccessfully)
        {
            var response = await result;
            return Ok(response.Message.Select(res => new GetNurseApiResponse(res.Id, res.FirstName, res.LastName)));
        }

        var domainError = await errors;
        throw domainError.Message.ToException();
    }
    
    [HttpGet("GetWaitingPatients")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<PatientWaitingApiResponse>))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetWaitingPatients()
    {
        var result = await _getWaitingPatients.GetResponse<WaitingPatientsForNurses[]>(new {});

        var response = result.Message.Select(x => new PatientWaitingApiResponse()
        {
            Id = x.PatientInformation.Id,
            FirstName = x.PatientInformation.FirstName,
            LastName = x.PatientInformation.LastName
        });
        
        return Ok(response);
    }
}