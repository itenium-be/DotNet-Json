using Newtonsoft.Json;
using System.Globalization;

namespace Itenium.Json.NewtonsoftJson.Converters;

public class CulturalMoneyConverter(CultureInfo culture) : Newtonsoft.Json.JsonConverter
{
    public override bool CanConvert(Type objectType) => objectType == typeof(decimal);

    public override object ReadJson(JsonReader reader, Type objectType, object? existingValue, JsonSerializer serializer)
    {
        return reader.TokenType == JsonToken.String
            ? decimal.Parse((string)reader.Value, NumberStyles.Any, culture)
            : Convert.ToDecimal(reader.Value);
    }

    public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer)
    {
        writer.WriteValue(((decimal)value).ToString("#,###.##", culture));
    }
}