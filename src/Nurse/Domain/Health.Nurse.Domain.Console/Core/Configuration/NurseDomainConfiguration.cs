using Health.Nurse.Domain.Storage.Sql.Core.Configuration;
using Health.Shared.Application.Broker.Configuration;

namespace Health.Nurse.Domain.Console.Core.Configuration;

public interface NurseDomainConfiguration
{
    NurseStorageConfiguration NurseStorage { get; }
    BrokerCredentialsConfiguration BrokerCredentials { get; }
}