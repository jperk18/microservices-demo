using Health.Shared.Domain.Core.Exceptions;
using Health.Shared.Domain.Core.Exceptions.InnerModels;

namespace Health.Appointment.Domain.Console.Core.Exceptions;

public class AppointmentDomainValidationException : DomainValidationException
{
    public AppointmentDomainValidationException(string message) : base(message)
    {
    }

    public AppointmentDomainValidationException(string message, IEnumerable<IDomainValidationFailure> errors) : base(message, errors)
    {
    }
}