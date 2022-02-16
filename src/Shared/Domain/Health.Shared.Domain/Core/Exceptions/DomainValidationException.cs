using Health.Shared.Domain.Core.Exceptions.InnerModels;

namespace Health.Shared.Domain.Core.Exceptions;

public abstract class DomainValidationException : Exception, IDomainValidationResultObject
{
    protected DomainValidationException(string message)
    {
        Message = message;
        Errors = new List<IDomainValidationFailure>();
    }

    protected DomainValidationException(string message, IEnumerable<IDomainValidationFailure>? errors)
    {
        Message = message;
        Errors = errors;
    }

    public IEnumerable<IDomainValidationFailure>? Errors { get; set; }
    public override string Message { get; }
}