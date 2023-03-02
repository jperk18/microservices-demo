using Health.Shared.Domain.Exceptions;
using Health.Shared.Domain.Exceptions.Models;
using Health.Shared.Workflow.Processes.Exceptions.Models;
using OriginatingService = Health.Shared.Workflow.Processes.Exceptions.Models.OriginatingService;

namespace Health.Patient.Domain.Console.Exceptions;

public static class Extensions
{
    public static WorkflowValidation ToWorkflowValidationObject(this PatientDomainValidationException dv)
    {
        if (dv.Errors == null)
            return new WorkflowValidationDto(OriginatingService.Patient, dv.Message);
        
        return new WorkflowValidationDto(OriginatingService.Patient, dv.Message)
        {
            Errors = dv.Errors.Select(x => new WorkflowValidationFailureDto(x.ErrorCode){ AttemptedValue = x.AttemptedValue, ErrorCode = x.ErrorCode, ErrorMessage = x.ErrorMessage, PropertyName = x.PropertyName, Severity = GetWorkflowSeverity(x.Severity) }).ToArray<WorkflowValidationFailure>()
        };
    } 
    
    public static WorkflowValidation ToWorkflowValidationObject(this DomainValidationException dv)
    {
        if (dv.Errors == null)
            return new WorkflowValidationDto(OriginatingService.Patient, dv.Message);
        
        return new WorkflowValidationDto(OriginatingService.Patient, dv.Message)
        {
            Errors = dv.Errors.Select(x => new WorkflowValidationFailureDto(x.ErrorCode){ AttemptedValue = x.AttemptedValue, ErrorCode = x.ErrorCode, ErrorMessage = x.ErrorMessage, PropertyName = x.PropertyName, Severity = GetWorkflowSeverity(x.Severity) }).ToArray<WorkflowValidationFailure>()
        };
    } 
    
    public static PatientDomainValidationException ToPatientValidationException(this FluentValidationException e)
    {
        return new PatientDomainValidationException(e.Message, e.Errors);
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