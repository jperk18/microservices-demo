using Health.Nurse.Domain.Storage.Sql.Configuration;
using Health.Shared.Application.Broker.Configuration;

namespace Health.Nurse.Domain.Console.Configuration;

public interface NurseDomainConfiguration
{
    NurseStorageConfiguration NurseStorage { get; }
    BrokerCredentialsConfiguration BrokerCredentials { get; }
}