using Health.Patient.Domain.Console.Exceptions;
using Health.Shared.Domain.Exceptions;
using Health.Shared.Domain.Services;

namespace Health.Patient.Domain.Console.Services;

public interface IPatientValidationService<in TRequest> : IValidationService<TRequest>
{
}

public class PatientValidationService<TRequest> : IPatientValidationService<TRequest>
{
    private readonly IValidationService<TRequest> _coreValidation;

    public PatientValidationService(IFluentValidationService<TRequest> coreValidation)
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
            throw e.ToPatientValidationException();
        }
    }
}