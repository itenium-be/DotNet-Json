using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Itenium.Json.SystemTextJson.Converters;

public class CulturalMoneyConverter(CultureInfo culture) : JsonConverter<decimal>
{
    public override decimal Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        return reader.TokenType == JsonTokenType.String
            ? decimal.Parse(reader.GetString(), NumberStyles.Any, culture)
            : reader.GetDecimal();
    }

    public override void Write(Utf8JsonWriter writer, decimal value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.ToString("#,###.##", culture));
    }
}