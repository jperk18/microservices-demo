namespace Health.Shared.Domain.Storage.Configuration;

public class SqlDatabaseConfiguration
{
    public SqlDatabaseConfiguration() { }
    public SqlType DbType { get; set; }
    public string? ConnectionString { get; set; }
}