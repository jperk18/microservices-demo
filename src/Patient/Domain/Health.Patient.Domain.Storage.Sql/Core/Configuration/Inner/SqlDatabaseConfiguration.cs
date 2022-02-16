namespace Health.Patient.Domain.Storage.Sql.Core.Configuration.Inner;

public class SqlDatabaseConfiguration
{
    public SqlDatabaseConfiguration() { }
    public SqlType DbType { get; set; }
    public string? ConnectionString { get; set; }
}