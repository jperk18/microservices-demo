using System.Net.Mime;
using Health.Patient.Common.Requests;
using Microsoft.AspNetCore.Mvc;

namespace Health.Patient.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PatientController : ControllerBase
{
    private readonly ILogger<PatientController> _logger;

    public PatientController(ILogger<PatientController> logger)
    {
        _logger = logger;
    }

    [HttpGet("WaitingPatients")]    
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<IPatientInfo>))]
    public async Task<IActionResult> WaitingPatients()
    {
        return Ok();
    }
    
    [HttpPost("Register")]
    [Consumes(MediaTypeNames.Application.Json)]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(IPatientInfo))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Register([FromBody] IPatient patient)
    {
        //Create Patient in Domain
        return new OkResult();
    }
    
    [HttpPost("CheckIn")]
    [Consumes(MediaTypeNames.Application.Json)]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CheckIn([FromQuery] Guid patientId)
    {
        //Call Event for check-in => Domain
        return Ok();
    }
}