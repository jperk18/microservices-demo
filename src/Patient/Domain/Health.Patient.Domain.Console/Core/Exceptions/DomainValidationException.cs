using Health.Shared.Domain.Exceptions;
using Health.Shared.Domain.Exceptions.Models;

namespace Health.Patient.Domain.Console.Core.Exceptions;

public class PatientDomainValidationException : DomainValidationException
{
    public PatientDomainValidationException(string message) : base(message)
    {
    }

    public PatientDomainValidationException(string message, IEnumerable<DomainValidationFailure> errors) : base(message, errors)
    {
    }
}