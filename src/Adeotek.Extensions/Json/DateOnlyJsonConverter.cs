using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Adeotek.Extensions.Json;

public class DateOnlyJsonConverter : JsonConverter<DateOnly>
{
    private const string DateOnlyFormat = "yyyy-MM-dd";
    private const string FullDateFormat = "yyyy-MM-ddTHH:mm:ss";

    public override DateOnly Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var stringValue = reader.GetString();
        if (string.IsNullOrEmpty(stringValue))
        {
            throw new JsonException("Null or empty string is not a valid value for DateOnly type");
        }

        return stringValue.Length == 10 ? 
            DateOnly.ParseExact(stringValue, DateOnlyFormat, CultureInfo.InvariantCulture) :
            DateOnly.FromDateTime(DateTime.ParseExact(stringValue, FullDateFormat, CultureInfo.InvariantCulture));
    }

    public override void Write(Utf8JsonWriter writer, DateOnly value, JsonSerializerOptions options) => 
        writer.WriteStringValue(value.ToString(DateOnlyFormat, CultureInfo.InvariantCulture));
}