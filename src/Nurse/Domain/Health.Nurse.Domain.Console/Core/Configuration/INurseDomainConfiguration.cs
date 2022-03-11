﻿using Health.Nurse.Domain.Storage.Sql.Core.Configuration;

namespace Health.Nurse.Domain.Console.Core.Configuration;

public interface INurseDomainConfiguration
{
    INurseStorageConfiguration NurseStorage { get; }
    IBrokerCredentialsConfiguration BrokerCredentials { get; }
}