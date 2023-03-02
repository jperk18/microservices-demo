using Health.Appointment.Domain.Console.Exceptions;
using Health.Shared.Domain.Exceptions;
using Health.Shared.Domain.Services;

namespace Health.Appointment.Domain.Console.Services;

public interface IAppointmentValidationService<in TRequest> : IValidationService<TRequest>
{
}

public class AppointmentValidationService<TRequest> : IAppointmentValidationService<TRequest>
{
    private readonly IValidationService<TRequest> _coreValidation;

    public AppointmentValidationService(IFluentValidationService<TRequest> coreValidation)
    {
        _coreValidation = coreValidation ?? throw new ArgumentNullException(nameof(coreValidation));
    }

    public async Task Validate(TRequest query)
    {
        try
        {
            await _coreValidation.Validate(query);
        }
        catch (FluentValidationException e)
        {
            throw e.ToAppointmentValidationException();
        }
    }
}