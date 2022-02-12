using Health.Patient.Domain.Console.Core;
using Health.Patient.Domain.Storage.Sql.Core;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;

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

// Add Services to Domain (and storage dependant service)
var storageSettings = builder.Configuration.GetSection("DomainConfiguration:StorageConfiguration:PatientDatabase").Get<SqlDatabaseConfiguration>();
builder.Services.AddDomainServices(new DomainConfiguration(new StorageConfiguration(storageSettings)));

var app = builder.Build();


app.Run();