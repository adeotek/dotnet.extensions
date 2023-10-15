using System.Text.Json;
using System.Text.Json.Serialization;

namespace Adeotek.Extensions.Json;

public static class JsonAsyncSerializer
{
    /// <summary>
    /// Asynchronously serialize an object to string.
    /// </summary>
    /// <param name="input">Object to be serialized</param>
    /// <param name="options">Optional JSON options</param>
    /// <returns>Serialized object string</returns>
    public static async Task<string> SerializeAsync(object input, JsonSerializerOptions? options = null)
    {
        using var stream = new MemoryStream();
        await JsonSerializer.SerializeAsync(stream, input, input.GetType(), options ?? DefaultJsonSerializerOptions());
        stream.Position = 0;
        using var reader = new StreamReader(stream);
        return await reader.ReadToEndAsync();
    }
        
    /// <summary>
    /// Asynchronously serialize an generic type object to string.
    /// </summary>
    /// <typeparam name="T">Generic Type of the object to be serialized</typeparam>
    /// <param name="input">Object instance to serialize.</param>
    /// <param name="options">Optional JSON options</param>
    /// <returns>Serialized object string</returns>
    public static async Task<string> SerializeAsync<T>(T input, JsonSerializerOptions? options = null)
    {
        using var stream = new MemoryStream();
        await JsonSerializer.SerializeAsync(stream, input, options ?? DefaultJsonSerializerOptions());
        stream.Position = 0;
        using var reader = new StreamReader(stream);
        return await reader.ReadToEndAsync();
    }
        
    /// <summary>
    /// Serialize an generic type object to string.
    /// </summary>
    /// <typeparam name="T">Generic Type of the object to be serialized</typeparam>
    /// <param name="input">Object instance to serialize.</param>
    /// <param name="options">Optional JSON options</param>
    /// <returns>Serialized object string</returns>
    public static string Serialize<T>(T input, JsonSerializerOptions? options = null) =>
        JsonSerializer.Serialize(input, options ?? DefaultJsonSerializerOptions());

    public static JsonSerializerOptions DefaultJsonSerializerOptions() =>
        new()
        {
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingDefault,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            WriteIndented = false,
            AllowTrailingCommas = true
        };
}