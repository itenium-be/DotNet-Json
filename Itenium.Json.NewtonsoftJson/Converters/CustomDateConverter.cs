using Newtonsoft.Json.Converters;

namespace Itenium.Json.NewtonsoftJson.Converters;

public class CustomDateTimeConverter : IsoDateTimeConverter
{
    public CustomDateTimeConverter()
    {
        // Just use: JsonSerializerSettings.DateFormatString?
        // But this can be applied to a single DateTime...
        DateTimeFormat = "yyyy-MM-dd";
    }
}

// Usage:
//[JsonConverter(typeof(CustomDateTimeConverter))]
//public DateTime BirthDate { get; set; }