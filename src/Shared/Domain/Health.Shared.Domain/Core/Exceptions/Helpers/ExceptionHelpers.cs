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

    public static DomainValidationException GetDomainValidationException(ValidationException e)
    {
        var errorsList = e.Errors.Select(x =>
        {
            var ser = GetDomainSeverity(x.Severity);
            return (IDomainValidationFailure) new DomainValidationFailure()
            {
                ErrorMessage = x.ErrorMessage,
                AttemptedValue = x.AttemptedValue,
                ErrorCode = x.ErrorCode,
                PropertyName = x.PropertyName,
                Severity = ser
            };
        });
        
        return new DomainValidationException(e.Message, errorsList);
    }

    private class DomainValidationFailure : IDomainValidationFailure
    {
        public string? PropertyName { get; set; }
        public string ErrorMessage { get; set; }
        public object? AttemptedValue { get; set; }
        public DomainSeverity Severity { get; set; }
        public string ErrorCode { get; set; }
    }

    private class DomainValidationResultObject : IDomainValidationResultObject
    {
        public string Message { get; set; }
        public IEnumerable<IDomainValidationFailure>? Errors { get; set; }
    }
}