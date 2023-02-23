using Health.Shared.Domain.Exceptions;
using Health.Shared.Domain.Exceptions.Models;

namespace Health.Nurse.Domain.Console.Exceptions;

public class NurseDomainValidationException : DomainValidationException
{
    public NurseDomainValidationException(string message) : base(message)
    {
    }

    public NurseDomainValidationException(string message, IEnumerable<DomainValidationFailure> errors) : base(message, errors)
    {
    }
}