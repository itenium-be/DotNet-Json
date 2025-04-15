using Itenium.Json.Models;
using Itenium.Json.NewtonsoftJson.Converters;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Decimal = Itenium.Json.Models.Decimal;
using Double = Itenium.Json.Models.Double;

namespace Itenium.Json.NewtonsoftJson;

public class NewtonsoftSettingsTests
{
    [Fact]
    public void Serialize_NullAndDefaultHandling()
    {
        var obj = new NullValues();
        string json = JsonConvert.SerializeObject(obj);
        Assert.Equal("{\"Integer\":0,\"NullableInteger\":null,\"Word\":null,\"NonNullableWord\":null,\"Boolean\":false,\"Enum\":0,\"NullableEnum\":null}", json);


        var sets = new JsonSerializerSettings()
        {
            DefaultValueHandling = DefaultValueHandling.Ignore,
        };
        json = JsonConvert.SerializeObject(obj, sets);
        Assert.Equal("{}", json);


        sets = new JsonSerializerSettings()
        {
            NullValueHandling = NullValueHandling.Ignore,
        };
        json = JsonConvert.SerializeObject(obj, sets);
        Assert.Equal("{\"Integer\":0,\"Boolean\":false,\"Enum\":0}", json);
    }

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
    public void Serialize_Indented()
    {
        var settings = new JsonSerializerSettings
        {
            Formatting = Formatting.Indented,
        };
        var obj = new EnumValue();
        string json = JsonConvert.SerializeObject(obj, settings);

        var expected = """
                       {
                         "Enum": 0
                       }
                       """;
        Assert.Equal(expected, json);
    }

    [Fact]
    public void Serialize_Floats_CustomConverter()
    {
        // Customer Converter for STJ NumberHandling
        var settings = new JsonSerializerSettings
        {
            Converters = new List<JsonConverter> { new StringFloatConverter() },
        };

        var obj = new Floats { Float = 3.1415f, Float2 = float.NaN};
        string json = JsonConvert.SerializeObject(obj, settings);
        Assert.Equal("{\"Float\":\"3.1415\",\"Float2\":\"NaN\"}", json);
    }

    [Fact]
    public void Serialize_Floats()
    {
        var obj = new Floats() { Float = float.NaN };
        string json = JsonConvert.SerializeObject(obj);
        Assert.Equal("{\"Float\":\"NaN\",\"Float2\":0.0}", json);


        var sets = new JsonSerializerSettings()
        {
           FloatFormatHandling = FloatFormatHandling.String,
        };
        obj = new Floats() { Float = 3.1415f, Float2 = float.NegativeInfinity };
        json = JsonConvert.SerializeObject(obj, sets);
        Assert.Equal("{\"Float\":3.1415,\"Float2\":\"-Infinity\"}", json);

        var dbl = new Double() { Value = double.PositiveInfinity };
        json = JsonConvert.SerializeObject(dbl, sets);
        Assert.Equal("{\"Value\":\"Infinity\"}", json);


        sets = new JsonSerializerSettings()
        {
            FloatFormatHandling = FloatFormatHandling.Symbol
        };
        obj = new Floats() { Float = 3.1415f, Float2 = float.NaN };
        json = JsonConvert.SerializeObject(obj, sets);
        Assert.Equal("{\"Float\":3.1415,\"Float2\":NaN}", json);
    }


    [Fact]
    public void DateAndTime()
    {
        var settings = new JsonSerializerSettings
        {
            // DateFormatString = "dd-MM-yyyy HH:mm:ss",
            DateFormatHandling = DateFormatHandling.IsoDateFormat,
            DateTimeZoneHandling = DateTimeZoneHandling.Utc,
            DateParseHandling = DateParseHandling.DateTimeOffset
        };

        var obj = new DateTimeContainer
        {
            DateTime = new DateTime(2023, 1, 15, 10, 30, 0, DateTimeKind.Local)
        };

        string json = JsonConvert.SerializeObject(obj, settings);
        Assert.Contains("Z", json);

        var result = JsonConvert.DeserializeObject<DateTimeContainer>(json, settings)!;
        Assert.Equal(DateTimeKind.Utc, result.DateTime.Kind);
    }

    [Fact]
    public void SameReferenceHandling_DoNotSerializeTwice()
    {
        var obj = new Node();
        obj.Next = obj;

        var settings = new JsonSerializerSettings
        {
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore
        };

        string json = JsonConvert.SerializeObject(obj, settings);
        Assert.Equal("{}", json);
    }

    [Fact]
    public void MissingMembers_ThrowException()
    {
        const string json = "{\"ExtraProperty\":42}";
        var obj = JsonConvert.DeserializeObject<Node>(json);
        Assert.NotNull(obj);

        var settings = new JsonSerializerSettings
        {
            MissingMemberHandling = MissingMemberHandling.Error
        };
        Assert.Throws<JsonSerializationException>(() => JsonConvert.DeserializeObject<Node>(json, settings));
    }

    [Fact]
    public void AllowPrivateConstructor()
    {
        const string json = "{\"Name\":\"Test\"}";
        var settings = new JsonSerializerSettings
        {
            ConstructorHandling = ConstructorHandling.AllowNonPublicDefaultConstructor
        };

        var obj = JsonConvert.DeserializeObject<ConstructorClass>(json, settings)!;
        Assert.Equal("Test", obj.Name);
    }


    [Fact]
    public void Serialize_AllSettings()
    {
        var settings = new JsonSerializerSettings
        {
            PreserveReferencesHandling = PreserveReferencesHandling.None,
            EqualityComparer = null,
            ReferenceResolverProvider = null,
            ObjectCreationHandling = ObjectCreationHandling.Auto,

            SerializationBinder = null,
            TypeNameHandling = TypeNameHandling.None,
            TypeNameAssemblyFormatHandling = TypeNameAssemblyFormatHandling.Simple,

            MetadataPropertyHandling = MetadataPropertyHandling.Default,
            CheckAdditionalContent = false,

            StringEscapeHandling = StringEscapeHandling.Default,
            ContractResolver = null,
            
            TraceWriter = null,
            Error = null,
            MaxDepth = null,

            Context = default,
            Culture = null,
        };
    }
}
