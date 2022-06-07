using Health.Appointment.Transports.Api.Models;
using Health.Shared.Workflow.Processes.Commands;
using Health.Shared.Workflow.Processes.Core.Exceptions;
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
    private readonly IRequestClient<RequestNurseAssignmentForAppointment> _requestNurseAssignmentForAppointmentRequestClient;
    private readonly IRequestClient<RequestScheduleAppointment> _requestScheduleAppointmentRequestClient;

    public AppointmentController(ILogger<AppointmentController> logger,
        IRequestClient<GetAllWaitingPatients> getAllWaitingPatientsRequestClient,
        IRequestClient<RequestScheduleAppointment> requestScheduleAppointmentRequestClient,
        IRequestClient<RequestNurseAssignmentForAppointment> requestNurseAssignmentForForAppointmentRequestClient)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _getAllWaitingPatientsRequestClient = getAllWaitingPatientsRequestClient ??
                                              throw new ArgumentNullException(nameof(getAllWaitingPatientsRequestClient));
        _requestScheduleAppointmentRequestClient = requestScheduleAppointmentRequestClient ?? throw new ArgumentNullException(nameof(requestScheduleAppointmentRequestClient));
        _requestNurseAssignmentForAppointmentRequestClient = requestNurseAssignmentForForAppointmentRequestClient ??
                                                             throw new ArgumentNullException(nameof(requestNurseAssignmentForForAppointmentRequestClient));
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
    
    [HttpPut("ScheduleAppointment")]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(ScheduleAppointmentApiResponse))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> ScheduleAppointment(ScheduleAppointmentApiRequest request)
    {
        var (result, errors) = await _requestScheduleAppointmentRequestClient
            .GetResponse<RequestScheduleAppointmentSuccess, RequestScheduleAppointmentFailed>(new
            {
                PatientId = request.Patient
            });

        if (result.IsCompletedSuccessfully)
        {
            var response = await result;
            return new ObjectResult(new ScheduleAppointmentApiResponse(){ Appointment = response.Message.AppointmentId })
                {StatusCode = StatusCodes.Status201Created};
        }

        var domainError = await errors;
        throw new WorkflowValidationException(domainError.Message.Error);
    }
    
    [HttpPut("AssignNurse")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<Guid>))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> AssignNurse(AssignNurseApiRequest request)
    {
        var (result, errors) = await _requestNurseAssignmentForAppointmentRequestClient
            .GetResponse<RequestNurseAssignmentForAppointmentSuccess, RequestNurseAssignmentForAppointmentFailed>(new
            {
                AppointmentId = request.Appointment,
                NurseId = request.Nurse
            });

        if (result.IsCompletedSuccessfully)
        {
            var response = await result;
            return Ok();
        }

        var domainError = await errors;
        throw new WorkflowValidationException(domainError.Message.Error);
    }
}