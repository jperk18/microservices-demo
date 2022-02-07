using Health.Patient.Domain.Core.Decorators;

namespace Health.Patient.Domain.Core.Registration;

public static class DecoratorRegistrationHelper
{
    public static Type ToDecorator(object attribute, Type assigningInterfaceType)
    {
        Type type = attribute.GetType();

        if (type == typeof(AuditLogPipelineAttribute))
        {
            if (HandlerRegistration.IsCommandHandlerInterface(assigningInterfaceType))
                return typeof(AuditLoggingCommandDecorator<,>);
            if (HandlerRegistration.IsQueryHandlerInterface(assigningInterfaceType))
                return typeof(AuditLoggingQueryDecorator<,>);

            throw new ArgumentException(attribute.ToString());
        }
        
        if (type == typeof(ValidationPipelineAttribute))
        {
            if (HandlerRegistration.IsCommandHandlerInterface(assigningInterfaceType))
                return typeof(ValidationCommandDecorator<,>);
            if (HandlerRegistration.IsQueryHandlerInterface(assigningInterfaceType))
                return typeof(ValidationQueryDecorator<,>);

            throw new ArgumentException(attribute.ToString());
        }

        // if (type == typeof(AuditLogPipelineAttribute))
        //     return typeof(AuditLoggingCommandDecorator<,>);

        // other attributes go here

        throw new ArgumentException(attribute.ToString());
    }
}