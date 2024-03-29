﻿using Health.Nurse.Domain.Console.Core.Decorators;
using Health.Shared.Domain.Core.Decorators;

namespace Health.Shared.Domain.Core.RegistrationHelpers;

public static class Decorators
{
    public static bool IsDecorator(object x)
    {
        return x is LoggingPipelineAttribute || x is ValidationPipelineAttribute || x is ExceptionPipelineAttribute; //|| x is TransactionPipelineAttribute
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