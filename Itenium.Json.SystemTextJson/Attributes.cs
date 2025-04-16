using System.Text.Json;
using System.Text.Json.Serialization;

namespace Itenium.Json.SystemTextJson;

public class PersonWithAttributes :
    IJsonOnSerializing, IJsonOnSerialized,
    IJsonOnDeserializing, IJsonOnDeserialized
{
    /// <summary>
    /// Serialized as person_name
    /// </summary>
    [JsonPropertyName("person_name")]
    [JsonPropertyOrder(10)]
    public string Name { get; }

    /// <summary>
    /// Null values are not serialized
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public int? Age { get; }

    /// <summary>
    /// Never serialized
    /// </summary>
    [JsonIgnore]
    public string Password { get; set; }

    //[JsonConverter(typeof(BirthDateFormatConverter))]
    //public DateTime BirthDate { get; }

    [JsonConstructor]
    public PersonWithAttributes(string name, int? age = null)
    {
        Name = name; // For STJ only "name" works, "person_name" does NOT
        Age = age; // For STJ not provided arguments need default value (ie: "= null") or does not work
        Password = "FromJsonCtor";
    }

    public PersonWithAttributes(string name, string secret)
    {
        Name = name;
        Password = secret;
    }

    void IJsonOnSerializing.OnSerializing()
    {
    }

    void IJsonOnSerialized.OnSerialized()
    {
    }

    void IJsonOnDeserializing.OnDeserializing()
    {
    }

    void IJsonOnDeserialized.OnDeserialized()
    {
    }
}
