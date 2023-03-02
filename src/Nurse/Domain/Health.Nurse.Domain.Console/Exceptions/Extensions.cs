using Health.Shared.Domain.Exceptions;
using Health.Shared.Domain.Exceptions.Models;
using Health.Shared.Workflow.Processes.Exceptions.Models;
using OriginatingService = Health.Shared.Workflow.Processes.Exceptions.Models.OriginatingService;

namespace Health.Nurse.Domain.Console.Exceptions;

public static class Extensions
{
    public static WorkflowValidation ToWorkflowValidationObject(this NurseDomainValidationException dv)
    {
        if (dv.Errors == null)
            return new WorkflowValidationDto(OriginatingService.Nurse, dv.Message);

        return new WorkflowValidationDto(OriginatingService.Nurse, dv.Message)
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
            return new WorkflowValidationDto(OriginatingService.Nurse, dv.Message);
        
        return new WorkflowValidationDto(OriginatingService.Nurse, dv.Message)
        {
            Errors = dv.Errors.Select(x => new WorkflowValidationFailureDto(x.ErrorCode)
            {
                AttemptedValue = x.AttemptedValue, ErrorCode = x.ErrorCode, ErrorMessage = x.ErrorMessage,
                PropertyName = x.PropertyName, Severity = GetWorkflowSeverity(x.Severity)
            }).ToArray<WorkflowValidationFailure>()
        };
    }

    public static NurseDomainValidationException ToNurseValidationException(this FluentValidationException e)
    {
        return new NurseDomainValidationException(e.Message, e.Errors);
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