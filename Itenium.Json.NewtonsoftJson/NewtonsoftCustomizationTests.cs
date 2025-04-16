using System.Globalization;
using Itenium.Json.Models;
using Itenium.Json.NewtonsoftJson.Converters;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Itenium.Json.NewtonsoftJson;

public class NewtonsoftCustomizationTests
{
    [Fact]
    public void Serialize_EnumAsString()
    {
        var settings = new JsonSerializerSettings
        {
            Converters = new List<JsonConverter> { new StringEnumConverter() }
        };
        var obj = new EnumValue
        {
            Enum = State.Closed
        };
        string json = JsonConvert.SerializeObject(obj, settings);
        Assert.Equal("{\"Enum\":\"Closed\"}", json);
    }

    [Fact]
    public void UnixEpochDateTime()
    {
        var opts = new JsonSerializerSettings()
        {
            DefaultValueHandling = DefaultValueHandling.Ignore
        };
        opts.Converters.Add(new UnixEpochDateTimeConverter());

        var obj = new DateTimeContainer()
        {
            DateTime = new DateTime(2024, 9, 18, 0, 0, 0, DateTimeKind.Utc)
        };
        string json = JsonConvert.SerializeObject(obj, opts);
        Assert.Equal("{\"DateTime\":1726617600}", json);


        obj = JsonConvert.DeserializeObject<DateTimeContainer>(json, opts)!;
        Assert.Equal(new DateTime(2024, 9, 18), obj.DateTime);
    }

    [Fact]
    public void CulturalMoneyConverter()
    {
        var opts = new JsonSerializerSettings();
        opts.Converters.Add(new CulturalMoneyConverter(new CultureInfo("nl-BE")));

        var obj = new Money()
        {
            Value = 1000.5m
        };
        string json = JsonConvert.SerializeObject(obj, opts);
        Assert.Equal("{\"Value\":\"1.000,5\"}", json);


        obj = JsonConvert.DeserializeObject<Money>(json, opts)!;
        Assert.Equal(1000.5m, obj.Value);
    }

    [Fact]
    public void AttributesTest()
    {
        var person = new PersonWithAttributes("John", "mySecret123");
        string json = JsonConvert.SerializeObject(person);
        Assert.Equal("{\"person_name\":\"John\"}", json);

        var deserialized = JsonConvert.DeserializeObject<PersonWithAttributes>(json)!;
        Assert.Equal("John", deserialized.Name);
        Assert.Equal("FromJsonCtor", deserialized.Password);
    }

    [Fact]
    public void Attributes_SerializationOrderTest()
    {
        var person = new PersonWithAttributes("John", 18);
        string json = JsonConvert.SerializeObject(person);
        Assert.Equal("{\"Age\":18,\"person_name\":\"John\"}", json);
    }
}
