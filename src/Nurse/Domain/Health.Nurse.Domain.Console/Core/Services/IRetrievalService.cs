namespace Health.Nurse.Domain.Console.Core.Services;

public interface IRetrievalService
{
    Task<string> Get();
}