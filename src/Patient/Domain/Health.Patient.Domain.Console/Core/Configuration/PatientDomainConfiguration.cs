using Health.Patient.Domain.Storage.Sql.Core.Configuration;
using Health.Shared.Application.Broker.Configuration;

namespace Health.Patient.Domain.Console.Core.Configuration;

public interface PatientDomainConfiguration
{
    PatientStorageConfiguration PatientStorage { get; set; }
    BrokerCredentialsConfiguration BrokerCredentials { get; }
}