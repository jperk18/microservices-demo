namespace Health.Shared.Domain.Storage.Configuration;

public interface SqlDatabaseConfiguration
{
    SqlType DbType { get; }
    string? ConnectionString { get; }
}

public class SqlDatabaseConfigurationDto : SqlDatabaseConfiguration
{
    public SqlDatabaseConfigurationDto() { }
    public SqlType DbType { get; set; }
    public string? ConnectionString { get; set; }
}