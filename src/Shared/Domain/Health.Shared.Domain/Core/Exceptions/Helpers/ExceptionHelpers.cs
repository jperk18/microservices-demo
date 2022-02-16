using FluentValidation;
using Health.Shared.Domain.Core.Exceptions.InnerModels;

namespace Health.Shared.Domain.Core.Exceptions.Helpers;

public static class ExceptionHelpers
{
    private static DomainSeverity GetDomainSeverity(FluentValidation.Severity exception) =>
        exception switch
        {
            FluentValidation.Severity.Error => DomainSeverity.Error,
            FluentValidation.Severity.Warning => DomainSeverity.Warning,
            FluentValidation.Severity.Info => DomainSeverity.Info,
            _ => DomainSeverity.Error,
        };

    public static IDomainValidationResultObject GetDomainValidationException(ValidationException e)
    {
        return (IDomainValidationResultObject)(object)new
        {
            Messages = e.Message,
            Errors = e.Errors.Select(x => (IDomainValidationFailure)(object) new {
                ErrorMessage = x.ErrorMessage,
                AttemptedValue = x.AttemptedValue,
                ErrorCode = x.ErrorCode,
                PropertyName = x.PropertyName,
                Severity = GetDomainSeverity(x.Severity)
            })
        };
    }
}