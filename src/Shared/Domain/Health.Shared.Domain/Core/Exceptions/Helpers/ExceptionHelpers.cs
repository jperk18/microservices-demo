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
            return (IDomainValidationFailure) new DomainValidationFailure(x.ErrorCode, x.ErrorMessage)
            {
                AttemptedValue = x.AttemptedValue,
                PropertyName = x.PropertyName,
                Severity = ser
            };
        });
        
        return new DomainValidationException(e.Message, errorsList);
    }

    private class DomainValidationFailure : IDomainValidationFailure
    {
        public DomainValidationFailure(string errorCode, string errorMessage)
        {
            ErrorMessage = errorMessage;
            ErrorCode = errorCode;
        }

        public string? PropertyName { get; set; }
        public string ErrorMessage { get; set; }
        public object? AttemptedValue { get; set; }
        public DomainSeverity Severity { get; set; }
        public string ErrorCode { get; set; }
    }

    private class DomainValidationResultObject : IDomainValidationResultObject
    {
        public DomainValidationResultObject(string message)
        {
            Message = message;
        }
        public DomainValidationResultObject(string message, IEnumerable<IDomainValidationFailure>? errors)
        {
            Message = message;
            Errors = errors;
        }

        public string Message { get; set; }
        public IEnumerable<IDomainValidationFailure>? Errors { get; set; }
    }
}