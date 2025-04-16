using System.Text.Json;
using System.Text.Json.Serialization;
using Itenium.Json.Models;

namespace Itenium.Json.SystemTextJson;

public class SystemTextOptionsTests
{
    [Fact]
    public void WebDefaults()
    {
        var opts = new JsonSerializerOptions(JsonSerializerDefaults.Web);
        Assert.True(opts.PropertyNameCaseInsensitive);
        Assert.Equal(JsonNamingPolicy.CamelCase, opts.PropertyNamingPolicy);
        Assert.Equal(JsonNumberHandling.AllowReadingFromString, opts.NumberHandling);
    }

    [Fact]
    public void Serialize_NullAndDefaultHandling()
    {
        var obj = new NullValues();
        string json = JsonSerializer.Serialize(obj);
        Assert.Equal("{\"Integer\":0,\"NullableInteger\":null,\"Word\":null,\"NonNullableWord\":null,\"Boolean\":false,\"Enum\":0,\"NullableEnum\":null}", json);


        var opts = new JsonSerializerOptions
        {
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingDefault
        };
        json = JsonSerializer.Serialize(obj, opts);
        Assert.Equal("{}", json);


        opts = new JsonSerializerOptions
        {
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
        };
        json = JsonSerializer.Serialize(obj, opts);
        Assert.Equal("{\"Integer\":0,\"Boolean\":false,\"Enum\":0}", json);
    }

    [Fact]
    public void Serialize_EnumAsString()
    {
        var opts = new JsonSerializerOptions();
        opts.Converters.Add(new JsonStringEnumConverter());
        var obj = new EnumValue()
        {
            Enum = State.Closed
        };
        string json = JsonSerializer.Serialize(obj, opts);
        Assert.Equal("{\"Enum\":\"Closed\"}", json);
    }

    [Fact]
    public void Serialize_Indented()
    {
        var opts = new JsonSerializerOptions()
        {
            WriteIndented = true,
            IndentCharacter = ' ',
            IndentSize = 2,
            NewLine = Environment.NewLine,
        };
        var obj = new EnumValue();
        string json = JsonSerializer.Serialize(obj, opts);
        var expected = """
                       {
                         "Enum": 0
                       }
                       """;
        Assert.Equal(expected, json);
    }

    [Fact]
    public void Serialize_FloatsAndDoubles()
    {
        // Same as JsonNumberHandling.Strict:
        var obj = new Floats() { Float = float.NaN };
        Assert.Throws<ArgumentException>(() => JsonSerializer.Serialize(obj));


        var opts = new JsonSerializerOptions()
        {
            NumberHandling = JsonNumberHandling.WriteAsString | JsonNumberHandling.AllowReadingFromString,
        };
        obj = new Floats() { Float = 3.1415f, Float2 = float.NegativeInfinity};
        string json = JsonSerializer.Serialize(obj, opts);
        Assert.Equal("{\"Float\":\"3.1415\",\"Float2\":\"-Infinity\"}", json);

        var dbl = new Itenium.Json.Models.Double() { Value = double.PositiveInfinity };
        json = JsonSerializer.Serialize(dbl, opts);
        Assert.Equal("{\"Value\":\"Infinity\"}", json);


        opts = new JsonSerializerOptions()
        {
            NumberHandling = JsonNumberHandling.AllowNamedFloatingPointLiterals,
        };
        obj = new Floats() { Float = 3.1415f, Float2 = float.NaN };
        json = JsonSerializer.Serialize(obj, opts);
        Assert.Equal("{\"Float\":3.1415,\"Float2\":\"NaN\"}", json);
    }

    [Fact]
    public void HandleTimeSpan()
    {
        var obj = new TimeSpanContainer
        {
            Value = TimeSpan.FromSeconds(61)
        };

        string json = JsonSerializer.Serialize(obj);
        Assert.Equal("{\"Value\":\"00:01:01\"}", json);

        var result = JsonSerializer.Deserialize<TimeSpanContainer>(json)!;
        Assert.Equal(61, result.Value.TotalSeconds);
    }

    [Fact]
    public void SameReferenceHandling_DoNotSerializeTwice()
    {
        // https://learn.microsoft.com/en-us/dotnet/standard/serialization/system-text-json/preserve-references

        var obj = new Node();
        obj.Next = obj;

        var options = new JsonSerializerOptions
        {
            ReferenceHandler = ReferenceHandler.IgnoreCycles
        };

        string json = JsonSerializer.Serialize(obj, options);
        Assert.Equal("{\"Next\":null}", json);
    }

    [Fact]
    public void MissingMembers_ThrowException()
    {
        string json = "{\"ExtraProperty\":42}";
        var obj = JsonSerializer.Deserialize<Node>(json);
        Assert.NotNull(obj);

        var options = new JsonSerializerOptions
        {
            UnmappedMemberHandling = JsonUnmappedMemberHandling.Disallow
        };
        Assert.Throws<JsonException>(() => JsonSerializer.Deserialize<Node>(json, options));
    }

    [Fact]
    public void RequiredConstructorParameters()
    {
        const string json = "{\"Name\":\"Test\"}";
        const string jsonWithoutRequiredCtorParams = "{}";
        var options = new JsonSerializerOptions
        {
            RespectRequiredConstructorParameters = false
        };
        var obj = JsonSerializer.Deserialize<ConstructorClass>(json, options)!;
        Assert.Equal("Test", obj.Name);
        obj = JsonSerializer.Deserialize<ConstructorClass>(jsonWithoutRequiredCtorParams, options)!;
        Assert.Null(obj.Name);

        options = new JsonSerializerOptions
        {
            RespectRequiredConstructorParameters = true
        };

        obj = JsonSerializer.Deserialize<ConstructorClass>(json, options)!;
        Assert.Equal("Test", obj.Name);
        Assert.Throws<JsonException>(() => JsonSerializer.Deserialize<ConstructorClass>(jsonWithoutRequiredCtorParams, options));
    }

    [Fact(Skip = "RemainingOptions")]
    public void Serialize_RemainingOptions()
    {
        var opts = new JsonSerializerOptions
        {
            DictionaryKeyPolicy = null,
            PropertyNamingPolicy = null,

            IgnoreReadOnlyProperties = false,
            IgnoreReadOnlyFields = false,
            IncludeFields = false,
            RespectNullableAnnotations = false,

            PreferredObjectCreationHandling = JsonObjectCreationHandling.Replace,
            TypeInfoResolver = null,
            UnknownTypeHandling = JsonUnknownTypeHandling.JsonElement,

            AllowOutOfOrderMetadataProperties = false,
            DefaultBufferSize = 0,
            Encoder = null,
            MaxDepth = 0,
        };
    }
}
