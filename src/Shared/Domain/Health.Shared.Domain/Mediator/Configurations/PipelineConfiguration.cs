namespace Health.Shared.Domain.Mediator.Configurations;

public interface PipelineConfiguration
{
    Type Pipeline { get; }
    Type? CommandHandler { get; }
    Type? QueryHandler { get; }
}

public class PipelineConfigurationDto : PipelineConfiguration
{
    public PipelineConfigurationDto(Type pipeline)
    {
        Pipeline = pipeline;
        CommandHandler = null;
        QueryHandler = null;
    }

    public Type Pipeline { get; set; }
    public Type? CommandHandler { get; set; }
    public Type? QueryHandler { get; set; }
}