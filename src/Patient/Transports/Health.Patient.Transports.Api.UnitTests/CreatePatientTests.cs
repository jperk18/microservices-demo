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

public class CreatePatientTests
{
    private PatientController? _controller;
    private readonly Mock<ILogger<Api.Controllers.PatientController>> _logger;
    private readonly Faker _faker;

    public CreatePatientTests()
    {
        _faker = new Faker();
        _logger = new Mock<ILogger<Api.Controllers.PatientController>>();
    }
    
    [Fact]
    public async Task Should_Return_Created_Patient()
    {
        //Arrange
        var testPatient = new Shared.Workflow.Processes.Inner.Models.PatientDto(_faker.Random.Guid(), _faker.Name.FirstName(),
            _faker.Name.LastName(), _faker.Person.DateOfBirth);

        await using var provider = new ServiceCollection()
            .AddSingleton<Shared.Workflow.Processes.Inner.Models.Patient>(testPatient)
            .AddMassTransitTestHarness(cfg =>
            {
                cfg.AddConsumer<MockRegisterPatientConsumer>();
            })
            .BuildServiceProvider(true);
        var harness = provider.GetRequiredService<ITestHarness>();
        await harness.Start();
        
        _controller = new Api.Controllers.PatientController(_logger.Object, harness.GetRequestClient<RegisterPatient>(), harness.GetRequestClient<GetAllPatients>(), harness.GetRequestClient<GetPatient>());
        
        //Act
        var result = await _controller.Register(new CreatePatientApiRequest(testPatient.FirstName, testPatient.LastName, testPatient.DateOfBirth));

        //Assert
        Assert.True(await harness.Sent.Any<RegisterPatientSuccess>());
        Assert.True(await harness.Consumed.Any<RegisterPatient>());
        var consumerHarness = harness.GetConsumerHarness<MockRegisterPatientConsumer>();
        Assert.True(await consumerHarness.Consumed.Any<RegisterPatient>());
        var responseItem =
            TestingExtensions.AssertResponseFromIActionResult<CreatePatientApiResponse>(StatusCodes.Status201Created, result);
        Assert.Equal(responseItem.PatientId, testPatient.Id);
    }
    
    [Fact]
    public async Task Should_Fail_On_Any_WorkflowException()
    {
        //Arrange
        var testPatient = new Shared.Workflow.Processes.Inner.Models.PatientDto(_faker.Random.Guid(), null!,
            _faker.Name.LastName(), _faker.Person.DateOfBirth);
        
        await using var provider = new ServiceCollection()
            .AddSingleton<Shared.Workflow.Processes.Inner.Models.Patient>(testPatient)
            .AddMassTransitTestHarness(cfg =>
            {
                cfg.AddConsumer<MockRegisterPatientConsumer>();
            })
            .BuildServiceProvider(true);
        var harness = provider.GetRequiredService<ITestHarness>();
        await harness.Start();
        
        _controller = new Api.Controllers.PatientController(_logger.Object, harness.GetRequestClient<RegisterPatient>(), harness.GetRequestClient<GetAllPatients>(), harness.GetRequestClient<GetPatient>());
        
        //Act
        var result = await Assert.ThrowsAsync<WorkflowValidationException>(() => _controller.Register(new CreatePatientApiRequest(testPatient.FirstName, testPatient.LastName, testPatient.DateOfBirth)));
        
        //Assert
        Assert.True(await harness.Sent.Any<RegisterPatientFailed>());
        Assert.True(await harness.Consumed.Any<RegisterPatient>());
        var consumerHarness = harness.GetConsumerHarness<MockRegisterPatientConsumer>();
        Assert.True(await consumerHarness.Consumed.Any<RegisterPatient>());
        Assert.NotNull(result);
        Assert.Equal("ERROR", result.Message);
    }
    
    private class MockRegisterPatientConsumer : IConsumer<RegisterPatient>
    {
        private Shared.Workflow.Processes.Inner.Models.Patient _result;
        public MockRegisterPatientConsumer(Shared.Workflow.Processes.Inner.Models.Patient result)
        {
            _result = result;
        }

        public async Task Consume(ConsumeContext<RegisterPatient> context)
        {
            if (string.IsNullOrEmpty(_result.FirstName)) {
                await context.RespondAsync<RegisterPatientFailed>(new
                {
                    Error = new{ Message = "ERROR" }
                });
            }
            else
            {
                await context.RespondAsync<RegisterPatientSuccess>(new
                {
                    DateOfBirth = _result.DateOfBirth,
                    Id = _result.Id,
                    FirstName = _result.FirstName,
                    LastName = _result.LastName
                });
            }
        }
    }
}