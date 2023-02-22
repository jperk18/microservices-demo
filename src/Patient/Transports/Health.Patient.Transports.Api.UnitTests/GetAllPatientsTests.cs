using System.Threading.Tasks;
using Bogus;
using Health.Patient.Transports.Api.Controllers;
using Health.Patient.Transports.Api.Models;
using Health.Patient.Transports.Api.UnitTests.Extensions;
using Health.Shared.Workflow.Processes.Commands;
using Health.Shared.Workflow.Processes.Queries;
using MassTransit;
using MassTransit.Testing;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace Health.Patient.Transports.Api.UnitTests;

public class GetAllPatientsTests
{
    private PatientController? _controller;
    private readonly Mock<ILogger<Api.Controllers.PatientController>> _logger;
    private readonly Faker _faker;

    public GetAllPatientsTests()
    {
        _faker = new Faker();
        _logger = new Mock<ILogger<Api.Controllers.PatientController>>();
    }
    
    [Fact]
    public async Task Should_Return_Patients()
    {
        //Arrange
        var testPatient = new Shared.Workflow.Processes.Inner.Models.PatientDto(_faker.Random.Guid(), _faker.Name.FirstName(),
            _faker.Name.LastName(), _faker.Person.DateOfBirth);

        await using var provider = new ServiceCollection()
            .AddSingleton<Shared.Workflow.Processes.Inner.Models.Patient>(testPatient)
            .AddMassTransitTestHarness(cfg =>
            {
                cfg.AddConsumer<MockGetAllPatientsConsumer>();
            })
            .BuildServiceProvider(true);
        var harness = provider.GetRequiredService<ITestHarness>();
        await harness.Start();
        
        _controller = new Api.Controllers.PatientController(_logger.Object, harness.GetRequestClient<RegisterPatient>(), harness.GetRequestClient<GetAllPatients>(), harness.GetRequestClient<GetPatient>());
        
            
        //Act
        var result = await _controller.GetAllPatients();

        //Assert
        Assert.True(await harness.Sent.Any<GetAllPatientsSuccess>());
        Assert.True(await harness.Consumed.Any<GetAllPatients>());
        var consumerHarness = harness.GetConsumerHarness<MockGetAllPatientsConsumer>();
        Assert.True(await consumerHarness.Consumed.Any<GetAllPatients>());
        var response =
            TestingExtensions.AssertResponseFromIActionResult<GetPatientApiResponse[]>(StatusCodes.Status200OK, result);
        var responseItem = Assert.Single(response);
        Assert.Equal(responseItem.PatientId, testPatient.Id);
        Assert.Equal(responseItem.FirstName, testPatient.FirstName);
        Assert.Equal(responseItem.LastName, testPatient.LastName);
        Assert.Equal(responseItem.DateOfBirth, testPatient.DateOfBirth);
    }
    
    private class MockGetAllPatientsConsumer : IConsumer<GetAllPatients>
    {
        private Shared.Workflow.Processes.Inner.Models.Patient _result;
        public MockGetAllPatientsConsumer(Shared.Workflow.Processes.Inner.Models.Patient result)
        {
            _result = result;
        }

        public async Task Consume(ConsumeContext<GetAllPatients> context)
        {
            await context.RespondAsync<GetAllPatientsSuccess>(new
            {
                Patients = new [] {_result}
            });
        }
    }
}