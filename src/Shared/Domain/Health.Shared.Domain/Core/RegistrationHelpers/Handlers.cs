using System.Reflection;
using Health.Shared.Domain.Commands.Core;
using Health.Shared.Domain.Core.Configurations;
using Health.Shared.Domain.Mediator;
using Health.Shared.Domain.Queries.Core;
using Microsoft.Extensions.DependencyInjection;

namespace Health.Shared.Domain.Core.RegistrationHelpers;

public static class Handlers
{
    public static void AddHandlers(this IServiceCollection services, IEnumerable<Type> handlerTypes,
        IEnumerable<IPipelineConfiguration>? additionalPipelinesForHandlers = null,
        IEnumerable<IPipelineConfiguration>? corePipelinesForHandlersOverriders = null)
    {
        foreach (var type in handlerTypes)
        {
            AddHandler(services, type, additionalPipelinesForHandlers, corePipelinesForHandlersOverriders);
        }

        services.AddTransient<IMediator, Mediator.Mediator>();
    }

    private static void AddHandler(IServiceCollection services, Type type,
        IEnumerable<IPipelineConfiguration>? additionalPipelinesForHandlers = null,
        IEnumerable<IPipelineConfiguration>? corePipelinesForHandlersOverriders = null)
    {
        object[] attributes = type.GetCustomAttributes(false)
            .Where(x => Decorators.IsDecorator(x) || (additionalPipelinesForHandlers != null &&
                                                      additionalPipelinesForHandlers.Any(p => p.Pipeline == x.GetType())))
            .ToArray();

        Type interfaceType = type.GetInterfaces().Single(y => IsHandlerInterface(y));

        List<Type> pipeline = attributes
            .Select(x =>
            {
                if (Decorators.IsDecorator(x)) //is Core decorator in shared library
                {
                    var corePipelineConfigurationOverride =
                        corePipelinesForHandlersOverriders?.FirstOrDefault(p => p.Pipeline == x.GetType());

                    if (corePipelineConfigurationOverride == null)
                        return Decorators.ToDecorator(x, interfaceType);
                    
                    if (IsCommandHandlerInterface(interfaceType) &&
                        corePipelineConfigurationOverride.CommandHandler != null)
                        return corePipelineConfigurationOverride.CommandHandler;
                    if (IsQueryHandlerInterface(interfaceType) && corePipelineConfigurationOverride.QueryHandler != null)
                        return corePipelineConfigurationOverride.QueryHandler;

                    throw new ApplicationException("Core pipelines handler behaviour not specified correctly");
                }

                if (additionalPipelinesForHandlers != null)
                {
                    var additionalPipelineConfigurationOverride =
                        additionalPipelinesForHandlers?.FirstOrDefault(p => p.Pipeline == x.GetType());
                    
                    if (IsCommandHandlerInterface(interfaceType) &&
                        additionalPipelineConfigurationOverride?.CommandHandler != null)
                        return additionalPipelineConfigurationOverride.CommandHandler;
                    if (IsQueryHandlerInterface(interfaceType) && additionalPipelineConfigurationOverride?.QueryHandler != null)
                        return additionalPipelineConfigurationOverride.QueryHandler;

                    throw new ApplicationException("Additional pipelines handler behaviour not specified correctly");
                }

                throw new ApplicationException("Pipeline behaviour not specified");
            })
            .Concat(new[] {type})
            .Reverse()
            .ToList();

        Func<IServiceProvider, object> factory = BuildPipeline(pipeline, interfaceType);

        services.AddTransient(interfaceType, factory);
    }

    private static Func<IServiceProvider, object> BuildPipeline(List<Type> pipeline, Type interfaceType)
    {
        List<ConstructorInfo> ctors = pipeline
            .Select(x =>
            {
                Type type = x.IsGenericType ? x.MakeGenericType(interfaceType.GenericTypeArguments) : x;
                return type.GetConstructors().Single();
            })
            .ToList();

        Func<IServiceProvider, object> func = provider =>
        {
            object? current = null;

            foreach (ConstructorInfo ctor in ctors)
            {
                List<ParameterInfo> parameterInfos = ctor.GetParameters().ToList();

                object[] parameters = GetParameters(parameterInfos, current, provider);

                current = ctor.Invoke(parameters);
            }

            return current ?? new Object();
        };

        return func;
    }

    private static object[] GetParameters(List<ParameterInfo> parameterInfos, object? current,
        IServiceProvider provider)
    {
        var result = new object[parameterInfos.Count];

        for (int i = 0; i < parameterInfos.Count; i++)
        {
            result[i] = GetParameter(parameterInfos[i], current, provider);
        }

        return result;
    }

    private static object GetParameter(ParameterInfo parameterInfo, object? current, IServiceProvider provider)
    {
        Type parameterType = parameterInfo.ParameterType;

        if (IsHandlerInterface(parameterType))
            return current ?? new Object();

        object? service = provider.GetService(parameterType);
        if (service != null)
            return service;

        throw new ArgumentException($"Type {parameterType} not found");
    }

    public static bool IsHandlerInterface(Type type)
    {
        if (!type.IsGenericType)
            return false;

        Type typeDefinition = type.GetGenericTypeDefinition();

        return typeDefinition == typeof(IAsyncCommandHandler<,>) || typeDefinition == typeof(IAsyncQueryHandler<,>);
    }

    public static bool IsCommandHandlerInterface(Type type)
    {
        if (!type.IsGenericType)
            return false;

        Type typeDefinition = type.GetGenericTypeDefinition();

        return typeDefinition == typeof(IAsyncCommandHandler<,>);
    }

    public static bool IsQueryHandlerInterface(Type type)
    {
        if (!type.IsGenericType)
            return false;

        var typeDefinition = type.GetGenericTypeDefinition();

        return type.GetGenericTypeDefinition() == typeof(IAsyncQueryHandler<,>);
    }
}