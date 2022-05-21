using Health.Patient.Domain.Storage.Sql.Core.Configuration;
using Health.Shared.Application.Configuration;

namespace Health.Patient.Domain.Console.Core.Configuration;

public class PatientDomainConfiguration : IPatientDomainConfiguration
{
    public PatientDomainConfiguration(IPatientStorageConfiguration storage, IBrokerCredentialsConfiguration brokerCredentials)
    {
        PatientStorage = storage ?? throw new ArgumentNullException(nameof(storage));
        BrokerCredentials = brokerCredentials ?? throw new ArgumentNullException(nameof(brokerCredentials));
    }
    public IPatientStorageConfiguration PatientStorage { get; set; }
    public IBrokerCredentialsConfiguration BrokerCredentials { get; }
}