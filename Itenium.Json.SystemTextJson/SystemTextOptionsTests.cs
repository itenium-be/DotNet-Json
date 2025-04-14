using System.Text.Json;
using System.Text.Json.Serialization;
using Itenium.Json.Models;

namespace Itenium.Json.SystemTextJson;

public class SystemTextOptionsTests
{
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
        // This also works for doubles

        var obj = new Floats() { Float = float.NaN };
        Assert.Throws<ArgumentException>(() => JsonSerializer.Serialize(obj));


        var opts = new JsonSerializerOptions()
        {
            NumberHandling = JsonNumberHandling.WriteAsString | JsonNumberHandling.AllowReadingFromString,
        };
        obj = new Floats() { Float = 3.1415f, Float2 = float.NegativeInfinity};
        string json = JsonSerializer.Serialize(obj, opts);
        Assert.Equal("{\"Float\":\"3.1415\",\"Float2\":\"-Infinity\"}", json);


        opts = new JsonSerializerOptions()
        {
            NumberHandling = JsonNumberHandling.AllowNamedFloatingPointLiterals,
        };
        obj = new Floats() { Float = 3.1415f, Float2 = float.NaN };
        json = JsonSerializer.Serialize(obj, opts);
        Assert.Equal("{\"Float\":3.1415,\"Float2\":\"NaN\"}", json);
    }





    [Fact(Skip = "RemainingOptions")]
    public void Serialize_AllOptions()
    {
        var opts = new JsonSerializerOptions
        {
            DictionaryKeyPolicy = null,
            PropertyNamingPolicy = null,

            IgnoreReadOnlyProperties = false,
            IgnoreReadOnlyFields = false,
            IncludeFields = false,
            RespectNullableAnnotations = false,
            RespectRequiredConstructorParameters = false,

            TypeInfoResolver = null,
            AllowOutOfOrderMetadataProperties = false,
            DefaultBufferSize = 0,
            Encoder = null,
            PreferredObjectCreationHandling = JsonObjectCreationHandling.Replace,
            MaxDepth = 0,
            UnknownTypeHandling = JsonUnknownTypeHandling.JsonElement,
            UnmappedMemberHandling = JsonUnmappedMemberHandling.Skip,
            ReferenceHandler = null,
        };
    }
}
