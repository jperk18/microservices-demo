using Health.Patient.Transports.Api.Core.Configuration;
using Health.Patient.Transports.Api.Middleware;
using Health.Shared.Application;
using Health.Shared.Application.Configuration;
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
var brokerSettings = builder.Configuration.GetSection("PatientApi:BrokerCredentials").Get<BrokerCredentialsConfiguration>();
var config = new PatientApiConfiguration(brokerSettings);
builder.Services.AddSingleton<IPatientApiConfiguration>(config);

builder.Services.AddSharedApplicationServices();
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
    
    cfg.AddRequestClient<RegisterPatient>();
    cfg.AddRequestClient<GetPatient>();
    cfg.AddRequestClient<GetAllPatients>();
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