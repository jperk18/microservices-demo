﻿using System.Text.Json;

namespace Health.Shared.Domain.Core.Serialization;

public class JsonSerializer : IJsonSerializer
{
    private readonly JsonSerializerOptions _settings = new JsonSerializerOptions()
    {
        WriteIndented = true,
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
    };
    
    public string Serialize(object value)
    {
        return System.Text.Json.JsonSerializer.Serialize(value, _settings);
    }
    
    public T? Deserialize<T>(string value)
    {
        return System.Text.Json.JsonSerializer.Deserialize<T>(value, _settings) ?? default(T);
    }
}