using Health.Patient.Domain.Storage.Sql.Configuration;
using Health.Shared.Application.Broker.Configuration;

namespace Health.Patient.Domain.Console.Configuration;

public interface PatientDomainConfiguration
{
    PatientStorageConfiguration PatientStorage { get; set; }
    BrokerCredentialsConfiguration BrokerCredentials { get; }
}