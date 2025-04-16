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

public class TimeSpanContainer
{
    public TimeSpan Value { get; set; }
}

public class Floats
{
    public float Float { get; set; }
    public float Float2 { get; set; }
}

public class Double
{
    public double Value { get; set; }
}

public class Decimal
{
    public decimal Value { get; set; }
}

public class Money
{
    public decimal Value { get; set; }
}

public class Node
{
    public object Next { get; set; }
}

public class ConstructorClass
{
    public string Name { get; set; }
    public int Value { get; set; }
    public string CtorUsed { get; set; }

    public ConstructorClass(string name, int value = 0)
    {
        // STJ: RespectRequiredConstructorParameters = true
        Name = name;
        Value = value;
        CtorUsed = "Parameterized";
    }

    private ConstructorClass()
    {
        // Newtonsoft: ConstructorHandling.AllowNonPublicDefaultConstructor
        CtorUsed = "PrivateDefault";
    }
}