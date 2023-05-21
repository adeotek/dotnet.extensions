namespace Adeotek.StrongEnums.Example;

public class StringEnum : StrongEnumBase<StringEnum, string>
{
    public static readonly StringEnum V1 = new("Version 1", "v1.0");
    public static readonly StringEnum V2 = new("Version 2", "v2.0");
    public static readonly StringEnum V3 = new("Version 3", "v3.0");

    private StringEnum(string name, string value)
        : base(name, value)
    {
    }
}