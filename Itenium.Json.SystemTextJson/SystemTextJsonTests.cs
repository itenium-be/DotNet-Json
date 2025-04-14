using System.Text.Json;
using Itenium.Json.Models;

namespace Itenium.Json.SystemTextJson;

public class SystemTextJsonTests
{
    [Fact]
    public void Serialize()
    {
        var obj = new Person("Bert");
        string json = JsonSerializer.Serialize(obj);
        Assert.Equal("{\"Name\":\"Bert\"}", json);
    }

    [Fact]
    public void Deserialize()
    {
        const string json = "{\"Name\":\"Bert\"}";
        var obj = JsonSerializer.Deserialize<Person>(json)!;
        Assert.Equal("Bert", obj.Name);
    }

    [Fact]
    public void Deserialize_IsCaseSensitive()
    {
        const string json = "{\"name\":\"Bert\"}";
        var obj = JsonSerializer.Deserialize<Person>(json)!;
        Assert.Null(obj.Name);
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
        Assert.Throws<JsonException>(() => JsonSerializer.Deserialize<Person>(json));
    }

    [Fact]
    public void Deserialize_MissingKeyQuotes()
    {
        const string json = """
                            {
                                Name: "Bert"
                            }
                            """;
        Assert.Throws<JsonException>(() => JsonSerializer.Deserialize<Person>(json));
    }

    [Fact]
    public void Deserialize_SingleValueQuotes()
    {
        const string json = """
                            {
                                "Name": 'Bert'
                            }
                            """;
        Assert.Throws<JsonException>(() => JsonSerializer.Deserialize<Person>(json));
    }

    [Fact]
    public void Deserialize_ExtraComma()
    {
        const string json = """
                            {
                                "Name": "Bert",
                            }
                            """;
        Assert.Throws<JsonException>(() => JsonSerializer.Deserialize<Person>(json));
    }

    [Fact]
    public void Deserialize_Allows_Comments_CaseInsensitivity_ExtraComma_ViaConfiguration()
    {
        var opts = new JsonSerializerOptions()
        {
            AllowTrailingCommas = true,
            PropertyNameCaseInsensitive = true,
            ReadCommentHandling = JsonCommentHandling.Skip,
        };

        const string json = """
                            {
                                // This is a comment
                                "name": "Bert",
                            }
                            """;

        var obj = JsonSerializer.Deserialize<Person>(json, opts)!;
        Assert.Equal("Bert", obj.Name);
    }
}
