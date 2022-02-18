﻿using Health.Nurse.Domain.Storage.Sql.Core.Configuration;

namespace Health.Nurse.Domain.Console.Core.Configuration;

public class NurseDomainConfiguration : INurseDomainConfiguration
{
    public NurseDomainConfiguration(INurseStorageConfiguration storageConfiguration)
    {
        NurseStorageConfiguration = storageConfiguration ?? throw new ArgumentNullException(nameof(storageConfiguration));
    }
    public INurseStorageConfiguration NurseStorageConfiguration { get; set; }
}