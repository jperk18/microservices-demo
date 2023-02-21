using System.Reflection;
using Health.Shared.Domain.Mediator.Commands;
using Health.Shared.Domain.Mediator.Configurations;
using Health.Shared.Domain.Mediator.Decorators;
using Health.Shared.Domain.Mediator.Queries;
using Microsoft.Extensions.DependencyInjection;

namespace Health.Shared.Domain.Mediator;

public static class DependencyInjection
{
    public static void AddMediatorServices(this IServiceCollection services, IEnumerable<Type> handlerTypes,
        IEnumerable<PipelineConfiguration>? additionalPipelinesForHandlers = null,
        IEnumerable<PipelineConfiguration>? corePipelinesForHandlersOverriders = null)
    {
        foreach (var type in handlerTypes)
        {
            Handlers.AddHandler(services, type, additionalPipelinesForHandlers, corePipelinesForHandlersOverriders);
        }

        services.AddTransient<IMediator, Domain.Mediator.Mediator>();
    }
    
    public static class Handlers
    {
        public static void AddHandler(IServiceCollection services, Type type,
            IEnumerable<PipelineConfiguration>? additionalPipelinesForHandlers = null,
            IEnumerable<PipelineConfiguration>? corePipelinesForHandlersOverriders = null)
        {
            object[] attributes = type.GetCustomAttributes(false)
                .Where(x => Decorators.IsDecorator(x) || (additionalPipelinesForHandlers != null &&
                                                          additionalPipelinesForHandlers.Any(p =>
                                                              p.Pipeline == x.GetType())))
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
                        if (IsQueryHandlerInterface(interfaceType) &&
                            corePipelineConfigurationOverride.QueryHandler != null)
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
                        if (IsQueryHandlerInterface(interfaceType) &&
                            additionalPipelineConfigurationOverride?.QueryHandler != null)
                            return additionalPipelineConfigurationOverride.QueryHandler;

                        throw new ApplicationException(
                            "Additional pipelines handler behaviour not specified correctly");
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

    private static class Decorators
    {
        public static bool IsDecorator(object x)
        {
            return x is LoggingPipelineAttribute || x is ValidationPipelineAttribute || x is ExceptionPipelineAttribute;
        }

        public static Type ToDecorator(object attribute, Type assigningInterfaceType)
        {
            Type type = attribute.GetType();

            if (type == typeof(LoggingPipelineAttribute))
            {
                if (Handlers.IsCommandHandlerInterface(assigningInterfaceType))
                    return typeof(LoggingCommandDecorator<,>);
                if (Handlers.IsQueryHandlerInterface(assigningInterfaceType))
                    return typeof(LoggingQueryDecorator<,>);
            }

            if (type == typeof(ValidationPipelineAttribute))
            {
                if (Handlers.IsCommandHandlerInterface(assigningInterfaceType))
                    return typeof(ValidationCommandDecorator<,>);
                if (Handlers.IsQueryHandlerInterface(assigningInterfaceType))
                    return typeof(ValidationQueryDecorator<,>);
            }

            if (type == typeof(ExceptionPipelineAttribute))
            {
                if (Handlers.IsCommandHandlerInterface(assigningInterfaceType))
                    return typeof(ExceptionCommandDecorator<,>);
                if (Handlers.IsQueryHandlerInterface(assigningInterfaceType))
                    return typeof(ExceptionQueryDecorator<,>);
            }

            // other core attributes go here

            throw new ArgumentException(attribute.ToString());
        }
    }
}