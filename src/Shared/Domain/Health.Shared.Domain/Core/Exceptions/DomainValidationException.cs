using Health.Shared.Domain.Core.Exceptions.InnerModels;

namespace Health.Shared.Domain.Core.Exceptions;

public class DomainValidationException : Exception, IDomainValidationResultObject
{
    public DomainValidationException(string message)
    {
        Message = message;
        Errors = new List<IDomainValidationFailure>();
    }

    public DomainValidationException(string message, IEnumerable<IDomainValidationFailure> errors)
    {
        Message = message;
        Errors = errors;
    }

    public IEnumerable<IDomainValidationFailure>? Errors { get; set; }
    public override string Message { get; }
}