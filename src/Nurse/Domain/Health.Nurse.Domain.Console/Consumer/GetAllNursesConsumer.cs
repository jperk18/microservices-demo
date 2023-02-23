using Health.Nurse.Domain.Storage.Sql;
using Health.Shared.Workflow.Processes.Queries;
using MassTransit;

namespace Health.Nurse.Domain.Console.Consumer;

public class GetAllNursesConsumer : IConsumer<GetAllNurses>
{
    private readonly INurseRepository _nurseRepository;

    public GetAllNursesConsumer(INurseRepository nurseRepository)
    {
        _nurseRepository = nurseRepository ?? throw new ArgumentNullException(nameof(nurseRepository));
    }

    public async Task Consume(ConsumeContext<GetAllNurses> context)
    {
        var r = _nurseRepository.Nurses.GetAll();
        var nurses = r as Storage.Sql.Core.Databases.NurseDb.Models.Nurse[] ?? r.ToArray();

        await context.RespondAsync<GetAllNursesSuccess>(new
        {
            Nurses = nurses.Select(result =>
                new Shared.Workflow.Processes.Inner.Models.NurseDto(result.Id, result.FirstName, result.LastName,
                    result.DateOfBirth))
        });
    }
}