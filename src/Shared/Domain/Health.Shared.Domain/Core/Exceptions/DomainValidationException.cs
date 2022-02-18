using Health.Shared.Domain.Core.Exceptions.InnerModels;

namespace Health.Shared.Domain.Core.Exceptions;

public class DomainValidationException : Exception, IDomainValidationResultObject
{
    public DomainValidationException()
    {
        Message = "DOMAIN_ERROR";
        Errors = new List<IDomainValidationFailure>();
    }
    public DomainValidationException(string message)
    {
        Message = message ?? throw new ArgumentNullException(nameof(message));
        Errors = new List<IDomainValidationFailure>();
    }

    public DomainValidationException(string message, IEnumerable<IDomainValidationFailure> errors)
    {
        Message = message ?? throw new ArgumentNullException(nameof(message));
        Errors = errors ?? throw new ArgumentNullException(nameof(errors));
    }

    public IEnumerable<IDomainValidationFailure>? Errors { get; set; }
    public override string Message { get; }
}