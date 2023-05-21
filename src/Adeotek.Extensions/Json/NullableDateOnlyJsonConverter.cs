using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Adeotek.Extensions.Json;

public class NullableDateOnlyJsonConverter : JsonConverter<DateOnly?>
{
    private const string DateOnlyFormat = "yyyy-MM-dd";
    private const string FullDateFormat = "yyyy-MM-ddTHH:mm:ss";

    public override DateOnly? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var stringValue = reader.GetString();
        if (stringValue is null)
        {
            return null;
        }
        
        if (stringValue == "")
        {
            throw new JsonException("Empty string is not a valid value for DateOnly? type");
        }

        return stringValue.Length == 10 ? 
            DateOnly.ParseExact(stringValue, DateOnlyFormat, CultureInfo.InvariantCulture) :
            DateOnly.FromDateTime(DateTime.ParseExact(stringValue, FullDateFormat, CultureInfo.InvariantCulture));
    }

    public override void Write(Utf8JsonWriter writer, DateOnly? value, JsonSerializerOptions options)
    {
        if (value is null)
        {
            writer.WriteNullValue();
        }
        else
        {
            writer.WriteStringValue(value.Value.ToString(DateOnlyFormat, CultureInfo.InvariantCulture));
        }
    }
}