using Health.Shared.Domain.Exceptions;
using Health.Shared.Domain.Exceptions.Models;
using Health.Shared.Workflow.Processes.Core.Exceptions.Models;

namespace Health.Appointment.Domain.Console.Core.Exceptions.Helpers;

public static class ExceptionHelpers
{
    public static WorkflowValidation ToWorkflowValidationObject(this AppointmentDomainValidationException dv)
    {
        if (dv.Errors == null)
            return new WorkflowValidationDto(dv.Message);

        return new WorkflowValidationDto(dv.Message)
        {
            Errors = dv.Errors.Select(x => new WorkflowValidationFailureDto(x.ErrorCode)
            {
                AttemptedValue = x.AttemptedValue, ErrorCode = x.ErrorCode, ErrorMessage = x.ErrorMessage,
                PropertyName = x.PropertyName, Severity = GetWorkflowSeverity(x.Severity)
            }).ToArray<WorkflowValidationFailure>()
        };
    }

    public static WorkflowValidation ToWorkflowValidationObject(this DomainValidationException dv)
    {
        if (dv.Errors == null)
            return new WorkflowValidationDto(dv.Message);
        
        return new WorkflowValidationDto(dv.Message)
        {
            Errors = dv.Errors.Select(x => new WorkflowValidationFailureDto(x.ErrorCode)
            {
                AttemptedValue = x.AttemptedValue, ErrorCode = x.ErrorCode, ErrorMessage = x.ErrorMessage,
                PropertyName = x.PropertyName, Severity = GetWorkflowSeverity(x.Severity)
            }).ToArray<WorkflowValidationFailure>()
        };
    }

    private static WorkflowSeverity GetWorkflowSeverity(DomainSeverity exception) =>
        exception switch
        {
            DomainSeverity.Error => WorkflowSeverity.Error,
            DomainSeverity.Warning => WorkflowSeverity.Warning,
            DomainSeverity.Info => WorkflowSeverity.Info,
            _ => WorkflowSeverity.Error,
        };
}