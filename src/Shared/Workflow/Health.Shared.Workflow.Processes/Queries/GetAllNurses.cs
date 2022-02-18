using Health.Shared.Workflow.Processes.Inner.Models;

namespace Health.Shared.Workflow.Processes.Queries;

public interface GetAllNurses
{
}

public interface GetAllNursesSuccess
{
    Nurse[] Nurses { get; }
}