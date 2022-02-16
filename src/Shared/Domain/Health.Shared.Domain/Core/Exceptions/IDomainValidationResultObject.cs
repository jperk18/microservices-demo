using Health.Shared.Domain.Core.Exceptions.InnerModels;

namespace Health.Shared.Domain.Core.Exceptions;

public interface IDomainValidationResultObject
{
    string Message { get; }
    IEnumerable<IDomainValidationFailure>? Errors { get; set; }
}