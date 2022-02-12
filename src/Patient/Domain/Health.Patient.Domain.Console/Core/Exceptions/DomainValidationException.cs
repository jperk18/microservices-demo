using Health.Patient.Domain.Console.Core.Exceptions.Models;

namespace Health.Patient.Domain.Console.Core.Exceptions;

public class DomainValidationException : Exception, IDomainValidation
{
    public DomainValidationException(string message)
    {
        Message = message;
        Errors = new List<DomainValidationFailure>();
    }

    public DomainValidationException(string message, IEnumerable<DomainValidationFailure>? errors)
    {
        Message = message;
        Errors = errors;
    }

    public DomainValidation ToValidationObject()
    {
        return new DomainValidation(Message)
        {
            Errors = Errors
        };
    }

    public IEnumerable<DomainValidationFailure>? Errors { get; set; }
    public override string Message { get; }
}