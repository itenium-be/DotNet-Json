using Newtonsoft.Json.Converters;

namespace Itenium.Json.NewtonsoftJson.Converters;

public class CustomDateTimeConverter : IsoDateTimeConverter
{
    public CustomDateTimeConverter()
    {
        DateTimeFormat = "yyyy-MM-dd";
    }
}

// Usage:
//[JsonConverter(typeof(CustomDateTimeConverter))]
//public DateTime BirthDate { get; set; }