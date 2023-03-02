using Health.Shared.Domain.Exceptions.Models;

namespace Health.Shared.Domain.Exceptions;

public class FluentValidationException : Exception, DomainValidationResultObject
{
    public FluentValidationException(string message)
    {
        Message = message ?? throw new ArgumentNullException(nameof(message));
        Errors = new List<DomainValidationFailure>();
    }

    public FluentValidationException(string message, IEnumerable<DomainValidationFailure> errors)
    {
        Message = message ?? throw new ArgumentNullException(nameof(message));
        Errors = errors ?? throw new ArgumentNullException(nameof(errors));
    }

    public IEnumerable<DomainValidationFailure> Errors { get; set; }
    public override string Message { get; }
}