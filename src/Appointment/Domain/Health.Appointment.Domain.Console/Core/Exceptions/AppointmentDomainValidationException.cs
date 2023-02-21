using Health.Shared.Domain.Exceptions;
using Health.Shared.Domain.Exceptions.Models;

namespace Health.Appointment.Domain.Console.Core.Exceptions;

public class AppointmentDomainValidationException : DomainValidationException
{
    public AppointmentDomainValidationException(string message) : base(message)
    {
    }

    public AppointmentDomainValidationException(string message, IEnumerable<DomainValidationFailure> errors) : base(message, errors)
    {
    }
}