using System.Text.Json;
using System.Text.Json.Serialization;

namespace Adeotek.Extensions.Json;

public static class JsonAsyncSerializer
{
    /// <summary>
    /// Serialize an object to a string asynchronously, with our preferred options applied (ignore null values, camelcase naming).
    /// </summary>
    /// <param name="objectToSerialize">Object instance to serialize.</param>
    /// <param name="options">Optional JSON options if wanting to override default behavior.</param>
    /// <returns>String after serialization.</returns>
    public static async Task<string> SerializeAsync(object objectToSerialize, JsonSerializerOptions? options = null)
    {
        using var stream = new MemoryStream();
        options ??= new JsonSerializerOptions
        {
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingDefault,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };
        await JsonSerializer.SerializeAsync(stream, objectToSerialize, objectToSerialize.GetType(), options);
        stream.Position = 0;
        using var reader = new StreamReader(stream);
        return await reader.ReadToEndAsync();
    }
        
    /// <summary>
    /// Serialize an object to a string asynchronously, with our preferred options applied (ignore null values, camelcase naming).
    /// </summary>
    /// <typeparam name="T">Type of object to serialize.</typeparam>
    /// <param name="objectToSerialize">Object instance to serialize.</param>
    /// <param name="options">Optional JSON options if wanting to override default behavior.</param>
    /// <returns>String after serialization.</returns>
    public static async Task<string> SerializeAsync<T>(T objectToSerialize, JsonSerializerOptions? options = null)
    {
        using var stream = new MemoryStream();
        options ??= new JsonSerializerOptions
        {
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingDefault,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };
        await JsonSerializer.SerializeAsync(stream, objectToSerialize, options);
        stream.Position = 0;
        using var reader = new StreamReader(stream);
        return await reader.ReadToEndAsync();
    }
        
    /// <summary>
    /// Serialize an object to a string, with our preferred options applied (ignore null values, camelcase naming).
    /// </summary>
    /// <typeparam name="T">Type of object to serialize.</typeparam>
    /// <param name="objectToSerialize">Object instance to serialize.</param>
    /// <param name="options">Optional JSON options if wanting to override default behavior.</param>
    /// <returns>String after serialization.</returns>
    public static string Serialize<T>(T objectToSerialize, JsonSerializerOptions? options = null)
    {
        options ??= new JsonSerializerOptions
        {
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingDefault,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };
        return JsonSerializer.Serialize(objectToSerialize, options);
    }
}