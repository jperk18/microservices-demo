﻿namespace Health.Patient.Domain.Storage.Sql.Core;

public interface IStorageConfiguration
{
    SqlDatabaseConfiguration PatientDatabase { get; set; }
}

public class StorageConfiguration : IStorageConfiguration
{
    public StorageConfiguration(SqlDatabaseConfiguration patientDatabaseConfiguration)
    {
        PatientDatabase = patientDatabaseConfiguration ?? throw new ArgumentNullException(nameof(patientDatabaseConfiguration));
    }
    
    public SqlDatabaseConfiguration PatientDatabase { get; set; }
}

public class SqlDatabaseConfiguration
{
    public SqlDatabaseConfiguration() { }
    public SqlType DbType { get; set; }
    public string? ConnectionString { get; set; }
}

public enum SqlType
{
    Postgres = 0,
    InMemory = 99
}