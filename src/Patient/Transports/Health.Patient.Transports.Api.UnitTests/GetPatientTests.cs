using System.Threading.Tasks;
using Bogus;
using Health.Patient.Transports.Api.Controllers;
using Health.Patient.Transports.Api.Models;
using Health.Patient.Transports.Api.UnitTests.Extensions;
using Health.Shared.Workflow.Processes.Commands;
using Health.Shared.Workflow.Processes.Core.Exceptions;
using Health.Shared.Workflow.Processes.Queries;
using MassTransit;
using MassTransit.Testing;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace Health.Patient.Transports.Api.UnitTests;

public class GetPatientTests
{
    private PatientController? _controller;
    private readonly Mock<ILogger<Api.Controllers.PatientController>> _logger;
    private readonly Faker _faker;

    public GetPatientTests()
    {
        _faker = new Faker();
        _logger = new Mock<ILogger<Api.Controllers.PatientController>>();
    }
    
    [Fact]
    public async Task Should_Return_Patient_With_Id()
    {
        //Arrange
        var testPatient = new Shared.Workflow.Processes.Inner.Models.PatientDto(_faker.Random.Guid(), _faker.Name.FirstName(),
            _faker.Name.LastName(), _faker.Person.DateOfBirth);

        await using var provider = new ServiceCollection()
            .AddSingleton<Shared.Workflow.Processes.Inner.Models.Patient>(testPatient)
            .AddMassTransitTestHarness(cfg =>
            {
                cfg.AddConsumer<MockGetPatientConsumer>();
            })
            .BuildServiceProvider(true);
        var harness = provider.GetRequiredService<ITestHarness>();
        await harness.Start();
        
        _controller = new Api.Controllers.PatientController(_logger.Object, harness.GetRequestClient<RegisterPatient>(), harness.GetRequestClient<GetAllPatients>(), harness.GetRequestClient<GetPatient>());
        
            
        //Act
        var result = await _controller.GetPatient(new GetPatientApiRequest(){ PatientId = NewId.NextGuid()});

        //Assert
        Assert.True(await harness.Sent.Any<GetPatientSuccess>());
        Assert.True(await harness.Consumed.Any<GetPatient>());
        var consumerHarness = harness.GetConsumerHarness<MockGetPatientConsumer>();
        Assert.True(await consumerHarness.Consumed.Any<GetPatient>());
        var responseItem =
            TestingExtensions.AssertResponseFromIActionResult<GetPatientApiResponse>(StatusCodes.Status200OK, result);
        Assert.Equal(responseItem.PatientId, testPatient.Id);
        Assert.Equal(responseItem.FirstName, testPatient.FirstName);
        Assert.Equal(responseItem.LastName, testPatient.LastName);
        Assert.Equal(responseItem.DateOfBirth, testPatient.DateOfBirth);
    }
    
    [Fact]
    public async Task Should_Fail_On_Any_WorkflowException()
    {
        //Arrange
        var testPatient = new Shared.Workflow.Processes.Inner.Models.PatientDto(_faker.Random.Guid(), null,
            _faker.Name.LastName(), _faker.Person.DateOfBirth);
        
        await using var provider = new ServiceCollection()
            .AddSingleton<Shared.Workflow.Processes.Inner.Models.Patient>(testPatient)
            .AddMassTransitTestHarness(cfg =>
            {
                cfg.AddConsumer<MockGetPatientConsumer>();
            })
            .BuildServiceProvider(true);
        var harness = provider.GetRequiredService<ITestHarness>();
        await harness.Start();
        
        _controller = new Api.Controllers.PatientController(_logger.Object, harness.GetRequestClient<RegisterPatient>(), harness.GetRequestClient<GetAllPatients>(), harness.GetRequestClient<GetPatient>());
        
            
        //Act
        var result = await Assert.ThrowsAsync<WorkflowValidationException>(() => _controller.GetPatient(new GetPatientApiRequest(){ PatientId = NewId.NextGuid()}));
        
        //Assert
        Assert.True(await harness.Sent.Any<GetPatientFailed>());
        Assert.True(await harness.Consumed.Any<GetPatient>());
        var consumerHarness = harness.GetConsumerHarness<MockGetPatientConsumer>();
        Assert.True(await consumerHarness.Consumed.Any<GetPatient>());
        Assert.NotNull(result);
        Assert.Equal("ERROR", result.Message);
    }
    
    private class MockGetPatientConsumer : IConsumer<GetPatient>
    {
        private Shared.Workflow.Processes.Inner.Models.Patient _result;
        public MockGetPatientConsumer(Shared.Workflow.Processes.Inner.Models.Patient result)
        {
            _result = result;
        }

        public async Task Consume(ConsumeContext<GetPatient> context)
        {
            if (string.IsNullOrEmpty(_result.FirstName)) {
                await context.RespondAsync<GetPatientFailed>(new
                {
                    Error = new{ Message = "ERROR" }
                });
            }
            else
            {
                await context.RespondAsync<GetPatientSuccess>(new
                {
                    Patient = _result
                });
            }
        }
    }
}