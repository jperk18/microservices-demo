namespace Health.Shared.Domain.Core.Services;

public class DbTransactionContextType : IDbTransactionContextType
{
    public DbTransactionContextType(Type dbContext)
    {
        DatabaseContextType = dbContext;
    }
    
    public Type DatabaseContextType { get; }
}