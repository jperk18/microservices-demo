namespace Health.Nurse.Transports.Api.Core.Serialization;

public interface IJsonSerializer
{
    string Serialize(object value);
    T? Deserialize<T>(string value);
}