using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Runtime.Serialization;

namespace Itenium.Json.NewtonsoftJson;

/// <summary>
/// OptOut: Need to explicitly use [JsonIgnore] to not serialize a property
/// OptIn: Need to explicitly use [JsonProperty] to serialize a property
/// Fields: Serialize all fields too
/// </summary>
[JsonObject(MemberSerialization.OptOut)]
public class PersonWithAttributes
{
    /// <summary>
    /// Serialized as person_name
    /// </summary>
    [JsonProperty("person_name", Order = 10)]
    public string Name { get; }

    /// <summary>
    /// Null values are not serialized
    /// </summary>
    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public int? Age { get; }

    /// <summary>
    /// Never serialized
    /// </summary>
    [JsonIgnore]
    public string Password { get; set; }

    //[JsonConverter(typeof(DateFormatConverter), "yyyy-MM-dd")]
    //public DateTime BirthDate { get; }

    [JsonConstructor]
    public PersonWithAttributes(string name, int? age)
    {
        Name = name; // This parameter could also be called person_name
        Age = age;
        Password = "FromJsonCtor";
    }

    public PersonWithAttributes(string name, string secret)
    {
        Name = name;
        Password = secret;
    }

    [OnSerializing]
    internal void OnSerializingMethod(StreamingContext context)
    {
    }

    [OnSerialized]
    internal void OnSerializedMethod(StreamingContext context)
    {
    }

    [OnDeserializing]
    internal void OnDeserializingMethod(StreamingContext context)
    {
    }

    [OnDeserialized]
    internal void OnDeserializedMethod(StreamingContext context)
    {
    }
}
