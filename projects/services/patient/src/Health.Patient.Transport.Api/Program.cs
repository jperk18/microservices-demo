using Health.Patient.Api.Middleware;
using Health.Patient.Domain.Core;
using Health.Patient.Grpc.Services;
using Health.Patient.Transport.Api.Core;

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
var _apiSettings = new ApiConfiguration();
builder.Configuration.GetSection("ApiConfiguration").Bind(_apiSettings);
builder.Services.AddSingleton<IApiConfiguration>(_apiSettings);

builder.Services.AddControllers();
builder.Services.AddTransient<ExceptionHandlingMiddleware>();
builder.Services.AddSingleton<Health.Patient.Api.Core.Serialization.IJsonSerializer, Health.Patient.Api.Core.Serialization.JsonSerializer>();

// Add Services to Domain (and storage dependant service)
var _domainSettings = new DomainConfiguration();
builder.Configuration.GetSection("DomainConfiguration").Bind(_domainSettings);
builder.Services.AddDomainServices(_domainSettings);

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