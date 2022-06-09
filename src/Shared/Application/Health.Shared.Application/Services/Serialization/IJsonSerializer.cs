namespace Health.Shared.Application.Services.Serialization;

public interface IJsonSerializer
{
    string Serialize(object value);
    T? Deserialize<T>(string value);
}