namespace Health.Nurse.Domain.Console.Core.Exceptions.Models;

public class DomainValidation : IDomainValidation
{
    public DomainValidation(string message)
    {
        Message = message;
    }

    public IEnumerable<DomainValidationFailure>? Errors { get; set; }
    public string Message { get; set; }
    
    public DomainValidationException ToException()
    {
        return new DomainValidationException(Message, Errors);
    }
}