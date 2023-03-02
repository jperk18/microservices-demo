namespace Health.Shared.Domain.Services;

public interface IValidationService<in TRequest>
{
    Task Validate(TRequest request);
}