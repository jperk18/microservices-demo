using Health.Appointment.Transports.Api.Models;
using Health.Shared.Workflow.Processes.Commands;
using Health.Shared.Workflow.Processes.Exceptions;
using Health.Shared.Workflow.Processes.Queries;
using MassTransit;
using Microsoft.AspNetCore.Mvc;

namespace Health.Appointment.Transports.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AppointmentController : ControllerBase
{
    private readonly IRequestClient<GetAllWaitingPatients> _getAllWaitingPatientsRequestClient;
    private readonly IRequestClient<RequestNurseAssignmentForAppointment> _requestNurseAssignmentForAppointmentRequestClient;
    private readonly IRequestClient<RequestScheduleAppointment> _requestScheduleAppointmentRequestClient;
    private readonly IRequestClient<RequestPatientCheckIn> _requestPatientCheckInRequestClient;

    public AppointmentController(IRequestClient<GetAllWaitingPatients> getAllWaitingPatientsRequestClient,
        IRequestClient<RequestScheduleAppointment> requestScheduleAppointmentRequestClient,
        IRequestClient<RequestPatientCheckIn> requestPatientCheckInRequestClient,
        IRequestClient<RequestNurseAssignmentForAppointment> requestNurseAssignmentForForAppointmentRequestClient)
    {
        _getAllWaitingPatientsRequestClient = getAllWaitingPatientsRequestClient ??
                                              throw new ArgumentNullException(nameof(getAllWaitingPatientsRequestClient));
        _requestScheduleAppointmentRequestClient = requestScheduleAppointmentRequestClient ?? throw new ArgumentNullException(nameof(requestScheduleAppointmentRequestClient));
        _requestPatientCheckInRequestClient = requestPatientCheckInRequestClient ?? throw new ArgumentNullException(nameof(requestPatientCheckInRequestClient));
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
    
    [HttpPost("ScheduleAppointment")]
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
    
    [HttpPut("CheckIn")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(CheckInApiResponse))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> CheckIn(CheckInApiRequest request)
    {
        var (result, errors) = await _requestPatientCheckInRequestClient
            .GetResponse<RequestPatientCheckInSuccess, RequestPatientCheckInFailed>(new
            {
                AppointmentId = request.Appointment
            });

        if (result.IsCompletedSuccessfully)
        {
            var response = await result;
            return Ok(new CheckInApiResponse() {CheckedIn = true});
        }

        var domainError = await errors;
        throw new WorkflowValidationException(domainError.Message.Error);
    }
    
    [HttpPut("AssignNurse")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(AssignNurseApiResponse))]
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
            //var response = await result;
            return Ok(new AssignNurseApiResponse(){NurseAssigned = true});
        }

        var domainError = await errors;
        throw new WorkflowValidationException(domainError.Message.Error);
    }
}