using Itenium.Json.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Itenium.Json.NewtonsoftJson;

public class NewtonsoftJsonTests
{
    [Fact]
    public void Serialize()
    {
        var obj = new Person("Bert");
        string json = JsonConvert.SerializeObject(obj);
        Assert.Equal("{\"Name\":\"Bert\"}", json);
    }

    [Fact]
    public void Deserialize()
    {
        const string json = "{\"Name\":\"Bert\"}";
        var obj = JsonConvert.DeserializeObject<Person>(json)!;
        Assert.Equal("Bert", obj.Name);
    }

    [Fact]
    public void Deserialize_IsCaseInSensitive()
    {
        const string json = "{\"nAME\":\"Bert\"}";
        var obj = JsonConvert.DeserializeObject<Person>(json)!;
        Assert.Equal("Bert", obj.Name);
    }

    [Fact]
    public void Deserialize_Comments()
    {
        const string json = """
                            {
                                // This is a comment
                                "Name": "Bert"
                            }
                            """;
        var obj = JsonConvert.DeserializeObject<Person>(json)!;
        Assert.Equal("Bert", obj.Name);
    }

    [Fact]
    public void Deserialize_MissingKeyQuotes()
    {
        const string json = """
                            {
                                Name: "Bert"
                            }
                            """;
        var obj = JsonConvert.DeserializeObject<Person>(json)!;
        Assert.Equal("Bert", obj.Name);
    }

    [Fact]
    public void Deserialize_SingleValueQuotes()
    {
        const string json = """
                            {
                                "Name": 'Bert'
                            }
                            """;
        var obj = JsonConvert.DeserializeObject<Person>(json)!;
        Assert.Equal("Bert", obj.Name);
    }

    [Fact]
    public void Deserialize_ExtraComma()
    {
        const string json = """
                            {
                                "Name": "Bert",
                            }
                            """;
        var obj = JsonConvert.DeserializeObject<Person>(json)!;
        Assert.Equal("Bert", obj.Name);
    }

    [Fact]
    public void Deserialize_AllOfIt()
    {
        const string json = """
                            {
                                // Newtonsoft parses this
                                Name: 'Bert',
                            }
                            """;
        var obj = JsonConvert.DeserializeObject<Person>(json)!;
        Assert.Equal("Bert", obj.Name);
    }

    [Fact]
    public void DefaultSettings()
    {
        JsonConvert.DefaultSettings = () => new JsonSerializerSettings
        {
            Formatting = Formatting.Indented,
            ContractResolver = new CamelCasePropertyNamesContractResolver(),
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
            NullValueHandling = NullValueHandling.Ignore,
            DateTimeZoneHandling = DateTimeZoneHandling.Utc
        };
    }
}
