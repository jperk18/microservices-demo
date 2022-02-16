using Health.Patient.Transports.Api.Core;
using Health.Patient.Transports.Api.Core.Serialization;
using Health.Patient.Transports.Api.Middleware;
using Health.Workflow.Shared.Processes;
using MassTransit;
using MassTransit.Definition;
using MassTransit.RabbitMqTransport;
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
var apiSettings = builder.Configuration.GetSection("ApiConfiguration").Get<ApiConfiguration>();
builder.Services.AddSingleton<IApiConfiguration>(apiSettings);

builder.Services.AddTransient<ExceptionHandlingMiddleware>();
builder.Services.AddSingleton<IJsonSerializer, JsonSerializer>();

// Add Services to Domain (and storage dependant service)
//var storageSettings = builder.Configuration.GetSection("DomainConfiguration:StorageConfiguration:PatientDatabase").Get<SqlDatabaseConfiguration>();
//builder.Services.AddDomainServices(new DomainConfiguration(new StorageConfiguration(storageSettings)));
builder.Services.TryAddSingleton(KebabCaseEndpointNameFormatter.Instance);
builder.Services.AddMassTransit(cfg =>
{
    cfg.UsingRabbitMq(ConfigureBus);
    cfg.AddRequestClient<RegisterPatientCommandQuery>();
    cfg.AddRequestClient<GetPatientQuery>();
    cfg.AddRequestClient<GetAllPatientsQuery>();
    cfg.AddRequestClient<CheckInPatientCommandQuery>();
});

builder.Services.AddMassTransitHostedService();

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

static void ConfigureBus(IBusRegistrationContext context, IRabbitMqBusFactoryConfigurator configurator) {
    configurator.ConfigureEndpoints(context);
}