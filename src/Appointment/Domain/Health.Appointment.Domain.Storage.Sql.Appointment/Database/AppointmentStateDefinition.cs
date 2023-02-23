using Health.Appointment.Domain.StateMachines;
using MassTransit;

namespace Health.Appointment.Domain.Storage.Sql.Appointment.Database;

public class AppointmentStateDefinition :
    SagaDefinition<AppointmentState>
{
    readonly IServiceProvider _provider;
    public AppointmentStateDefinition(IServiceProvider provider)
    {
        _provider = provider;
    }
    protected override void ConfigureSaga(IReceiveEndpointConfigurator endpointConfigurator,
        ISagaConfigurator<AppointmentState> consumerConfigurator)
    {
        endpointConfigurator.UseMessageRetry(r => r.Intervals(100, 500, 1000, 1000, 1000, 1000, 1000));
        endpointConfigurator.UseEntityFrameworkOutbox<AppointmentStateDbContext>(_provider);
    }
}