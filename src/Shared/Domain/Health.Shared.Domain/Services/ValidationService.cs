using FluentValidation;
using Microsoft.Extensions.Logging;

namespace Health.Shared.Domain.Services;

public interface IValidationService<in TRequest>
{
    Task Validate(TRequest query);
}

public class ValidationService<TRequest> : IValidationService<TRequest>
{
    private readonly ILogger<ValidationService<TRequest>> _logger;
    private readonly IEnumerable<IValidator<TRequest>> _validators;

    public ValidationService(ILogger<ValidationService<TRequest>> logger, IEnumerable<IValidator<TRequest>> validators)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _validators = validators ?? throw new ArgumentNullException(nameof(validators));
    }
    
    public Task Validate(TRequest request)
    {
        var requestTypeName = typeof(TRequest).Name;
        try
        {
            if (!_validators.Any())
            {
                _logger.LogInformation("VALIDATION_SERVICE: No validation for {Name}. Continue with processing request...", requestTypeName);
                return Task.CompletedTask;
            }

            _logger.LogInformation("VALIDATION_SERVICE: Validator detected for {Name}. Running validators for {Name} request instance...", requestTypeName, requestTypeName);
            var context = new ValidationContext<TRequest>(request);
            var errors = _validators
                .Select(x => x.Validate(context))
                .SelectMany(x => x.Errors)
                .Where(x => x != null)
                .ToArray();

            if (errors.Any())
            {
                _logger.LogInformation("VALIDATION_SERVICE: Validators for {Name} request instance finished containing errors", requestTypeName);
                throw new ValidationException(errors);
            }

            _logger.LogInformation("VALIDATION_SERVICE: Validators for {Name} request instance finished successfully", requestTypeName);
        }
        catch (ValidationException e)
        {
            throw Shared.Domain.Exceptions.Extensions.GetDomainValidationException(e);
        }

        return Task.CompletedTask;
    }
}