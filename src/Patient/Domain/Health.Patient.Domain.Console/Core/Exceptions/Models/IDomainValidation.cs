namespace Health.Patient.Domain.Console.Core.Exceptions.Models;

public interface IDomainValidation
{
    IEnumerable<DomainValidationFailure>? Errors { get; set; }
}