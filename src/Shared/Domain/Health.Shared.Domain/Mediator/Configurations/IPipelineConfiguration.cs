namespace Health.Shared.Domain.Mediator.Configurations;

public interface IPipelineConfiguration
{
    Type Pipeline { get; }
    Type? CommandHandler { get; }
    Type? QueryHandler { get; }
}