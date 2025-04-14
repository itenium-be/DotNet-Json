namespace Itenium.Json.Models;

public enum State
{
    Open,
    Closed,
}

public class NullValues
{
    public int Integer { get; set; }
    public int? NullableInteger { get; set; }
    public string Word { get; set; }
    public string? NonNullableWord { get; set; }
    public bool Boolean { get; set; }
    public State Enum { get; set; }
    public State? NullableEnum { get; set; }
}

public class EnumValue
{
    public State Enum { get; set; }
}

public class DateTimeContainer
{
    public DateTime DateTime { get; set; }
    public DateTime? NullableDateTime { get; set; }
}

public class Floats
{
    public float Float { get; set; }
    public float Float2 { get; set; }
}
