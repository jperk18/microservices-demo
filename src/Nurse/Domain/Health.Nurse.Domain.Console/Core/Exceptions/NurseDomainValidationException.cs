using Health.Shared.Domain.Core.Exceptions;
using Health.Shared.Domain.Core.Exceptions.InnerModels;

namespace Health.Nurse.Domain.Console.Core.Exceptions;

public class NurseDomainValidationException : DomainValidationException
{
    public NurseDomainValidationException(string message) : base(message)
    {
    }

    public NurseDomainValidationException(string message, IEnumerable<IDomainValidationFailure>? errors) : base(message, errors)
    {
    }
}