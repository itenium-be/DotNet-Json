using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Json.Serialization.Metadata;
using System.Xml;
using Itenium.Json.Models;
using Itenium.Json.SystemTextJson.Converters;

namespace Itenium.Json.SystemTextJson;

public class SystemTextCustomizationTests
{
    [Fact]
    public void UnixEpochDateTime()
    {
        var opts = new JsonSerializerOptions()
        {
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
        };
        opts.Converters.Add(new UnixEpochDateTimeConverter());

        var obj = new DateTimeContainer()
        {
            DateTime = new DateTime(2024, 9, 18, 0, 0, 0, DateTimeKind.Utc)
        };
        string json = JsonSerializer.Serialize(obj, opts);
        Assert.Equal("{\"DateTime\":1726617600}", json);


        obj = JsonSerializer.Deserialize<DateTimeContainer>(json, opts)!;
        Assert.Equal(new DateTime(2024, 9, 18), obj.DateTime);
    }

    [Fact]
    public void CulturalMoneyConverter()
    {
        var opts = new JsonSerializerOptions();
        opts.Converters.Add(new CulturalMoneyConverter(new CultureInfo("nl-BE")));

        var obj = new Money()
        {
            Value = 1000.5m
        };
        string json = JsonSerializer.Serialize(obj, opts);
        Assert.Equal("{\"Value\":\"1.000,5\"}", json);


        obj = JsonSerializer.Deserialize<Money>(json, opts)!;
        Assert.Equal(1000.5m, obj.Value);
    }

    [Fact]
    public void AttributesTest()
    {
        var person = new PersonWithAttributes("John", "secret");
        string json = JsonSerializer.Serialize(person)!;
        Assert.Equal("{\"person_name\":\"John\"}", json);

        var deserialized = JsonSerializer.Deserialize<PersonWithAttributes>(json)!;
        Assert.Equal("John", deserialized.Name);
        Assert.Equal("FromJsonCtor", deserialized.Password);
    }

    [Fact]
    public void Attributes_SerializationOrderTest()
    {
        var person = new PersonWithAttributes("John", 18);
        string json = JsonSerializer.Serialize(person);
        Assert.Equal("{\"Age\":18,\"person_name\":\"John\"}", json);
    }
}
