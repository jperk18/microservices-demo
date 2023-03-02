using FluentValidation;
using Health.Shared.Domain.Exceptions.Models;

namespace Health.Shared.Domain.Exceptions;

public static class Extensions
{
    private static DomainSeverity GetDomainSeverity(FluentValidation.Severity exception) =>
        exception switch
        {
            FluentValidation.Severity.Error => DomainSeverity.Error,
            FluentValidation.Severity.Warning => DomainSeverity.Warning,
            FluentValidation.Severity.Info => DomainSeverity.Info,
            _ => DomainSeverity.Error,
        };

    public static DomainValidationException GetDomainValidationException(ValidationException e)
    {
        var errorsList = e.Errors.Select(x =>
        {
            var ser = GetDomainSeverity(x.Severity);
            return (DomainValidationFailure) new DomainValidationFailureDto(x.ErrorCode, x.ErrorMessage)
            {
                AttemptedValue = x.AttemptedValue,
                PropertyName = x.PropertyName,
                Severity = ser
            };
        });
        
        return new DomainValidationException(e.Message, errorsList);
    }
    
    public static FluentValidationException GetFluentValidationException(ValidationException e)
    {
        var errorsList = e.Errors.Select(x =>
        {
            var ser = GetDomainSeverity(x.Severity);
            return (DomainValidationFailure) new DomainValidationFailureDto(x.ErrorCode, x.ErrorMessage)
            {
                AttemptedValue = x.AttemptedValue,
                PropertyName = x.PropertyName,
                Severity = ser
            };
        });
        
        return new FluentValidationException(e.Message, errorsList);
    }
    
}