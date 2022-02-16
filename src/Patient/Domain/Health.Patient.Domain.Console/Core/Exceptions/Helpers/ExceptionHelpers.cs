using Health.Shared.Domain.Core.Exceptions;
using Health.Shared.Domain.Core.Exceptions.InnerModels;
using Health.Workflow.Shared.Processes.Core.Exceptions.Models;

namespace Health.Patient.Domain.Console.Core.Exceptions.Helpers;

public static class ExceptionHelpers
{
    public static WorkflowValidation ToWorkflowValidationObject(this PatientDomainValidationException dv)
    {
        return new WorkflowValidation(dv.Message)
        {
            Errors = dv.Errors.Select(x => new WorkflowValidationFailure(x.ErrorCode){ AttemptedValue = x.AttemptedValue, ErrorCode = x.ErrorCode, ErrorMessage = x.ErrorMessage, PropertyName = x.PropertyName, Severity = GetWorkflowSeverity(x.Severity) })
        };
    } 
    
    public static WorkflowValidation ToWorkflowValidationObject(this DomainValidationException dv)
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


}