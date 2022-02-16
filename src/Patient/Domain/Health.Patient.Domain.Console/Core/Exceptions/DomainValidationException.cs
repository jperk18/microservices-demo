using Health.Shared.Domain.Core.Exceptions;
using Health.Shared.Domain.Core.Exceptions.InnerModels;

namespace Health.Patient.Domain.Console.Core.Exceptions;

public class PatientDomainValidationException : DomainValidationException
{
    public PatientDomainValidationException(string message) : base(message)
    {
    }

    public PatientDomainValidationException(string message, IEnumerable<IDomainValidationFailure> errors) : base(message, errors)
    {
    }
}