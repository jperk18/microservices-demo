using FluentValidation.AspNetCore;
using Health.Patient.Api.Middleware;
using Health.Patient.Domain.Commands.CreatePatientCommand;
using Health.Patient.Domain.Core.Registration;
using Health.Patient.Grpc.Services;
using IJsonSerializer = Health.Patient.Domain.Core.Serialization.IJsonSerializer;
using JsonSerializer = Health.Patient.Domain.Core.Serialization.JsonSerializer;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container

builder.Services.AddControllers();
builder.Services.AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<CreatePatientCommandValidator>());
builder.Services.AddDomainServices();
builder.Services.AddTransient<ExceptionHandlingMiddleware>();
builder.Services.AddSingleton<Health.Patient.Api.Core.Serialization.IJsonSerializer, Health.Patient.Api.Core.Serialization.JsonSerializer>();

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