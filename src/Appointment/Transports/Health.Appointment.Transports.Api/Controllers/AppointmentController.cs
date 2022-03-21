using Health.Shared.Workflow.Processes.Queries;
using MassTransit;
using Microsoft.AspNetCore.Mvc;

namespace Health.Appointment.Transports.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AppointmentController : ControllerBase
{
    private readonly ILogger<AppointmentController> _logger;
    private readonly IRequestClient<GetAllWaitingPatients> _getAllWaitingPatientsRequestClient;

    public AppointmentController(ILogger<AppointmentController> logger,
        IRequestClient<GetAllWaitingPatients> getAllWaitingPatientsRequestClient
    )
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _getAllWaitingPatientsRequestClient = getAllWaitingPatientsRequestClient ??
                                              throw new ArgumentNullException(nameof(getAllWaitingPatientsRequestClient));
    }

    [HttpGet("AllWaitingPatients")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<Guid>))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetAllWaitingPatients()
    {
        var response = await _getAllWaitingPatientsRequestClient
            .GetResponse<GetAllWaitingPatientsSuccess>(new { });

        return Ok(response.Message.Patients);
    }
}