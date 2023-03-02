using Health.Nurse.Domain.Console.Exceptions;
using Health.Shared.Domain.Exceptions;
using Health.Shared.Domain.Services;

namespace Health.Nurse.Domain.Console.Services;

public interface INurseValidationService<in TRequest> : IValidationService<TRequest>
{
}

public class NurseValidationService<TRequest> : INurseValidationService<TRequest>
{
    private readonly IValidationService<TRequest> _coreValidation;

    public NurseValidationService(IFluentValidationService<TRequest> coreValidation)
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
            throw e.ToNurseValidationException();
        }
    }
}