using FluentValidation;
using Health.Nurse.Domain.Console.Core.Exceptions.Models;
using Health.Workflow.Shared.Processes.Core.Exceptions.Models;

namespace Health.Nurse.Domain.Console.Core.Exceptions.Helpers;

public static class ExceptionHelpers
{
    public static WorkflowValidation ToWorkflowValidationObject(this DomainValidation dv)
    {
        return new WorkflowValidation(dv.Message)
        {
            Errors = dv.Errors.Select(x => new WorkflowValidationFailure(x.ErrorCode){ AttemptedValue = x.AttemptedValue, ErrorCode = x.ErrorCode, ErrorMessage = x.ErrorMessage, PropertyName = x.PropertyName, Severity = GetWorkflowSeverity(x.Severity) })
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
    
    public static DomainSeverity GetDomainSeverity(FluentValidation.Severity exception) =>
        exception switch
        {
            FluentValidation.Severity.Error => DomainSeverity.Error,
            FluentValidation.Severity.Warning => DomainSeverity.Warning,
            FluentValidation.Severity.Info => DomainSeverity.Info,
            _ => DomainSeverity.Error,
        };

    public static DomainValidationException GetDomainValidationException(ValidationException e) => new DomainValidationException(e.Message)
    {
        Errors = e.Errors.Select(x => new DomainValidationFailure(x.ErrorMessage)
        {
            AttemptedValue = x.AttemptedValue,
            ErrorCode = x.ErrorCode,
            PropertyName = x.PropertyName,
            Severity = GetDomainSeverity(x.Severity)
        }).ToList()
    };
}