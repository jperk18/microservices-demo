namespace Health.Shared.Domain.Core.Configurations;

public interface IPipelineConfiguration
{
    Type Pipeline { get; }
    Type? CommandHandler { get; }
    Type? QueryHandler { get; }
}