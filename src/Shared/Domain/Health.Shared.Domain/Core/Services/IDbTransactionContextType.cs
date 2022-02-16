namespace Health.Shared.Domain.Core.Services;

public interface IDbTransactionContextType
{
    Type DatabaseContextType { get; }
}