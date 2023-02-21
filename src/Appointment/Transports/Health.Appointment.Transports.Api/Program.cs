using Health.Appointment.Transports.Api.Core.Configuration;
using Health.Appointment.Transports.Api.Middleware;
using Health.Shared.Application;
using Health.Shared.Application.Broker.Configuration;
using Health.Shared.Application.Serialization;
using Health.Shared.Workflow.Processes.Commands;
using Health.Shared.Workflow.Processes.Queries;
using MassTransit;
using Microsoft.Extensions.DependencyInjection.Extensions;

var builder = WebApplication.CreateBuilder(args);

// var preBuildConfig = new ConfigurationBuilder()
//     .AddJsonFile("appsettings.json", optional: false)
//     .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true, reloadOnChange: true)
//     .AddEnvironmentVariables()
//     .Build();

// Add Configuration to the container
builder.Configuration.AddJsonFile("appsettings.json", optional: false ,reloadOnChange: true);
builder.Configuration.AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true, reloadOnChange: true);
builder.Configuration.AddEnvironmentVariables();

// Add services to the container for API
var brokerSettings = builder.Configuration.GetSection("AppointmentApi:BrokerCredentials").Get<BrokerCredentialsConfiguration>();
var config = new AppointmentApiConfiguration(brokerSettings);
builder.Services.AddSingleton<IAppointmentApiConfiguration>(config);

builder.Services.AddSerializationServices();
builder.Services.AddTransient<ExceptionHandlingMiddleware>();

builder.Services.TryAddSingleton(KebabCaseEndpointNameFormatter.Instance);
builder.Services.AddMassTransit(cfg =>
{
    cfg.UsingRabbitMq((context, configurator) =>
    {
        configurator.Host(config.BrokerCredentials.Host, "/", h =>
        {
            h.Username(config.BrokerCredentials.Username);
            h.Password(config.BrokerCredentials.Password);
        });
        configurator.ConfigureEndpoints(context);
    });
    
    cfg.AddRequestClient<GetAllWaitingPatients>();
    cfg.AddRequestClient<RequestScheduleAppointment>();
    cfg.AddRequestClient<RequestPatientCheckIn>();
    cfg.AddRequestClient<RequestNurseAssignmentForAppointment>();
});

builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseMiddleware<ExceptionHandlingMiddleware>();
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();