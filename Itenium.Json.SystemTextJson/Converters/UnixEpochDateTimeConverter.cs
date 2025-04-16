using System.Text.Json;
using System.Text.Json.Serialization;

namespace Itenium.Json.SystemTextJson.Converters;

public class UnixEpochDateTimeConverter : JsonConverter<DateTime>
{
    private static readonly DateTime Epoch = new(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

    public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        return Epoch.AddSeconds(reader.GetInt64());
    }

    public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
    {
        writer.WriteNumberValue((long)(value.ToUniversalTime() - Epoch).TotalSeconds);
    }
}