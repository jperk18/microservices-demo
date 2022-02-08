namespace Health.Patient.Storage.Core;

public class StorageRegistrationConfiguration
{
    public StorageRegistrationConfiguration(DatabaseType dbtype, string? connectionString = null)
    {
        DbType = dbtype;
        ConnectionString = connectionString;
    }
    public DatabaseType DbType { get; set; }
    public string? ConnectionString { get; set; }

    public enum DatabaseType
    {
        InMemory = 0,
        Sql = 1
    }
}