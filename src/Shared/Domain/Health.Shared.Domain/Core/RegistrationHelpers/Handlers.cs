using System.Reflection;
using Health.Shared.Domain.Commands.Core;
using Health.Shared.Domain.Mediator;
using Health.Shared.Domain.Queries.Core;
using Microsoft.Extensions.DependencyInjection;

namespace Health.Shared.Domain.Core.RegistrationHelpers;

public static class Handlers
{
    public static void AddHandlers(this IServiceCollection services, IEnumerable<Type> handlerTypes,
        IDictionary<Type, Func<object, Type, Type?>>? additionalPipelinesForHandlers,
        IDictionary<Type, Func<object, Type, Type?>>? corePipelinesForHandlersOverriders = null)
    {
        foreach (var type in handlerTypes)
        {
            AddHandler(services, type, additionalPipelinesForHandlers, corePipelinesForHandlersOverriders);
        }

        services.AddTransient<IMediator, Mediator.Mediator>();
    }

    private static void AddHandler(IServiceCollection services, Type type,
        IDictionary<Type, Func<object, Type, Type?>>? additionalPipelinesForHandlers,
        IDictionary<Type, Func<object, Type, Type?>>? corePipelinesForHandlersOverriders = null)
    {
        object[] attributes = type.GetCustomAttributes(false)
            .Where(x => Decorators.IsDecorator(x) || (additionalPipelinesForHandlers != null && additionalPipelinesForHandlers.ContainsKey(x.GetType()))).ToArray();

        Type interfaceType = type.GetInterfaces().Single(y => IsHandlerInterface(y));

        List<Type> pipeline = attributes
            .Select(x =>
            {
                if (Decorators.IsDecorator(x)) //is Core decorator in shared library
                {
                    if (corePipelinesForHandlersOverriders != null && corePipelinesForHandlersOverriders.ContainsKey(x.GetType()))
                    {
                        var pipelineHandler = corePipelinesForHandlersOverriders[x.GetType()](x, interfaceType);

                        if (pipelineHandler == null)
                            throw new ApplicationException("Core pipelines handler behaviour not specified correctly");

                        return pipelineHandler;
                    }

                    return Decorators.ToDecorator(x, interfaceType);
                }

                if (additionalPipelinesForHandlers != null && additionalPipelinesForHandlers.ContainsKey(x.GetType()))
                {
                    var pipelineHandler = additionalPipelinesForHandlers[x.GetType()](x, interfaceType);

                    if (pipelineHandler == null)
                        throw new ApplicationException("Pipeline behaviour not specified");

                    return pipelineHandler;
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

    // private static Type ToDecorator(object attribute)
    // {
    //     Type type = attribute.GetType();
    //
    //     if (type == typeof(AuditLogPipelineAttribute))
    //         return typeof(AuditLoggingCommandDecorator<,>);
    //
    //     // other attributes go here
    //
    //     throw new ArgumentException(attribute.ToString());
    // }

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