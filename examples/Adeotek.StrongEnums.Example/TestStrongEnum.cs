using System.Reflection;

namespace Adeotek.StrongEnums.Example;

public abstract class TestStrongEnum<TEnum> : IEquatable<TestStrongEnum<TEnum>>
    where TEnum : TestStrongEnum<TEnum>
{
    private static readonly Dictionary<int, TEnum> _options = CreateEnumerations();
    
    public int Value { get; }
    public string Name { get; }

    protected TestStrongEnum(int value, string name)
    {
        Value = value;
        Name = name;
    }

    public static TEnum? FromValue(int value) =>
        _options.TryGetValue(value, out var enumeration) ?
            enumeration :
            default;

    public static TEnum? FromName(string? name) =>
        _options
            .Values
            .SingleOrDefault(e => e.Name == name);

    public bool Equals(TestStrongEnum<TEnum>? other) => 
        other is not null && GetType() == other.GetType() && Value == other.Value;

    public override bool Equals(object? obj) => 
        obj is TestStrongEnum<TEnum> other && Equals(other);

    public override int GetHashCode() => 
        Value.GetHashCode();

    private static Dictionary<int, TEnum> CreateEnumerations()
    {
        var enumType = typeof(TEnum);
        var fieldsForType = enumType
            .GetFields(
                BindingFlags.Public |
                BindingFlags.Static |
                BindingFlags.FlattenHierarchy)
            .Where(fi => enumType.IsAssignableFrom(fi.FieldType))
            .Select(fi => (TEnum)fi.GetValue(default)!);
        return fieldsForType.ToDictionary(x => x.Value);
    }
}