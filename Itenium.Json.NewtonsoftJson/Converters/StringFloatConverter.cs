using System.Globalization;
using Newtonsoft.Json;

namespace Itenium.Json.NewtonsoftJson.Converters;

/// <summary>
/// System.Text.Json has JsonSerializerOptions.NumberHandling
/// For Newtonsoft, we have to create a JsonConverter instead
/// </summary>
public class StringFloatConverter : JsonConverter
{
    public override bool CanConvert(Type objectType) => objectType == typeof(float);

    public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
    {
        if (reader.TokenType == JsonToken.String)
            return float.Parse((string)reader.Value);

        return Convert.ToDouble(reader.Value);
    }

    public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
    {
        writer.WriteValue(((float)value).ToString(CultureInfo.InvariantCulture));
    }
}