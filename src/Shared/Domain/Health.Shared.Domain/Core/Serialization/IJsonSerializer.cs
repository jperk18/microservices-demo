namespace Health.Shared.Domain.Core.Serialization;

public interface IJsonSerializer
{
    string Serialize(object value);
    T? Deserialize<T>(string value);
}