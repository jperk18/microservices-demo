namespace Health.Shared.Domain.Core.Configurations;

public class PipelineConfigurationDto : IPipelineConfiguration
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