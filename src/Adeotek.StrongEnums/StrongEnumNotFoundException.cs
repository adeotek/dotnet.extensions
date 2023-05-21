namespace Adeotek.StrongEnums;

[Serializable]
public class StrongEnumNotFoundException : Exception
{
    public static StrongEnumNotFoundException ThrowByName<TClass>(string? name)
        where TClass : IStrongEnum
    {
        return new StrongEnumNotFoundException($"No {typeof(TClass).Name} item with Name \"{name ?? "<null>"}\" found.");
    }
    
    public static StrongEnumNotFoundException ThrowByValue<TClass, TValue>(TValue? value)
        where TClass : IStrongEnum
        where TValue : IEquatable<TValue>, IComparable<TValue>
    {
        return new StrongEnumNotFoundException($"No {typeof(TClass).Name} item with Value {value?.ToString() ?? "<null>"} found.");
    }

    public StrongEnumNotFoundException(string message) : base(message)
    {
    }
}