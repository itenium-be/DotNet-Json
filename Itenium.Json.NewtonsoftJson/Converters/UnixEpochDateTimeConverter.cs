using Newtonsoft.Json;

namespace Itenium.Json.NewtonsoftJson.Converters;

public class UnixEpochDateTimeConverter : Newtonsoft.Json.JsonConverter
{
    private static readonly DateTime Epoch = new(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

    public override bool CanConvert(Type objectType)
    {
        return objectType == typeof(DateTime);
    }

    public override object ReadJson(JsonReader reader, Type objectType, object? existingValue, Newtonsoft.Json.JsonSerializer serializer)
    {
        return Epoch.AddSeconds((long)reader.Value);
    }

    public override void WriteJson(JsonWriter writer, object? value, Newtonsoft.Json.JsonSerializer serializer)
    {
        var dateTime = (DateTime)value;
        var unixTime = (long)(dateTime.ToUniversalTime() - Epoch).TotalSeconds;
        writer.WriteValue(unixTime);
    }
}