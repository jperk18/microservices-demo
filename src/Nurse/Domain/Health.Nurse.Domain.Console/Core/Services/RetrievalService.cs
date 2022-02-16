namespace Health.Nurse.Domain.Console.Core.Services;

public class RetrievalService : IRetrievalService
{
    public async Task<string> Get()
    {
        return await Task.FromResult("Hello");
    }
}