namespace Health.Shared.Domain.Exceptions.Models;

public interface DomainValidationResultObject
{
    string Message { get; }
    IEnumerable<DomainValidationFailure>? Errors { get; }
}