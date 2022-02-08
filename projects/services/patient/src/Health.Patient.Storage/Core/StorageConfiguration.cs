namespace Health.Patient.Storage.Core;

public interface IStorageConfiguration
{
    StorageConfiguration.DatabaseType DbType { get; set; }
    string? ConnectionString { get; set; }
}

public class StorageConfiguration : IStorageConfiguration
{
    public StorageConfiguration(DatabaseType dbtype, string? connectionString = null)
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