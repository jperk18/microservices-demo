using FluentValidation.AspNetCore;
using Health.Patient.Api.Middleware;
using Health.Patient.Domain.Commands.CreatePatientCommand;
using Health.Patient.Domain.Core;
using Health.Patient.Grpc.Services;
using Health.Patient.Storage.Core;

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
builder.Services.AddControllers();
builder.Services.AddTransient<ExceptionHandlingMiddleware>();
builder.Services.AddSingleton<Health.Patient.Api.Core.Serialization.IJsonSerializer, Health.Patient.Api.Core.Serialization.JsonSerializer>();

// Add Services to Domain (and storage depandant service)
builder.Services.AddDomainServices(new DomainRegistrationConfiguration(){ StorageConfiguration = new StorageRegistrationConfiguration(StorageRegistrationConfiguration.DatabaseType.InMemory)});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddGrpc();

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
// Configure the HTTP request pipeline.
app.MapGrpcService<GreeterService>();
//app.MapGet("/Grpc", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

app.Run();