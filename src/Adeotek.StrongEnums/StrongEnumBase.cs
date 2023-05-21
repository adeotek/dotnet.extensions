using System.Reflection;
using System.Runtime.CompilerServices;

namespace Adeotek.StrongEnums;

/// <summary>
/// Gets a collection containing all the instances of <see cref="StrongEnumBase{TClass, TValue}"/>.
/// </summary>
/// <value>A <see cref="IReadOnlyCollection{TClass}"/> containing all the instances of <see cref="StrongEnumBase{TClass, TValue}"/>.</value>
/// <remarks>Retrieves all the instances of <see cref="StrongEnumBase{TClass, TValue}"/> referenced by public static read-only fields in the current class or its bases.</remarks>
public abstract class StrongEnumBase<TClass, TValue> :
    IStrongEnum //,
    // IEquatable<StrongEnumBase<TClass, TValue>>,
    // IComparable<StrongEnumBase<TClass, TValue>>
    where TClass : StrongEnumBase<TClass, TValue>
    where TValue : IEquatable<TValue>, IComparable<TValue>
{
    /// <summary>
    /// Name property.
    /// </summary>
    /// <value>A <see cref="String"/> that is the name of the <see cref="StrongEnumBase{TClass, TValue}"/>.</value>
    public string Name { get; }

    /// <summary>
    /// Value property.
    /// </summary>
    /// <value>A <typeparamref name="TValue"/> that is the value of the <see cref="StrongEnumBase{TClass, TValue}"/>.</value>
    public TValue? Value { get; }
    
    protected StrongEnumBase(string name, TValue? value)
    {
        Name = name;
        Value = value;
    }
    
    /// <summary>
    /// Gets the item associated with the specified name.
    /// </summary>
    /// <param name="name">The name of the item to get.</param>
    /// <param name="ignoreCase"><c>true</c> to ignore case during the comparison; otherwise, <c>false</c>.</param>
    /// <returns>
    /// The item associated with the specified name. 
    /// If the specified name is not found, throws a <see cref="KeyNotFoundException"/>.
    /// </returns>
    /// <exception cref="ArgumentException"><paramref name="name"/> is <c>null</c>.</exception> 
    /// <exception cref="StrongEnumNotFoundException"><paramref name="name"/> does not exist.</exception> 
    /// <seealso cref="StrongEnumBase{TClass, TValue}.TryFromName(string, out TClass)"/>
    /// <seealso cref="StrongEnumBase{TClass, TValue}.TryFromName(string, bool, out TClass)"/>
    public static TClass FromName(string? name, bool ignoreCase = false)
    {
        if (string.IsNullOrEmpty(name))
        {
            throw new ArgumentException("Argument cannot be null or empty.", nameof(name));
        }

        return ignoreCase ?
            GetFromDictionary(_optionFromNameIgnoreCase.Value) :
            GetFromDictionary(_optionFromName.Value);

        TClass GetFromDictionary(IReadOnlyDictionary<string, TClass> dictionary)
        {
            if (!dictionary.TryGetValue(name, out var result))
            {
                throw StrongEnumNotFoundException.ThrowByName<TClass>(name);
            }
            
            return result;
        }
    }

    /// <summary>
    /// Gets the item associated with the specified name.
    /// </summary>
    /// <param name="name">The name of the item to get.</param>
    /// <param name="result">
    /// When this method returns, contains the item associated with the specified name, if the key is found; 
    /// otherwise, <c>null</c>. This parameter is passed uninitialized.</param>
    /// <returns>
    /// <c>true</c> if the <see cref="StrongEnumBase{TClass, TValue}"/> contains an item with the specified name; otherwise, <c>false</c>.
    /// </returns>
    /// <exception cref="ArgumentException"><paramref name="name"/> is <c>null</c>.</exception> 
    /// <seealso cref="StrongEnumBase{TClass, TValue}.FromName(string, bool)"/>
    /// <seealso cref="StrongEnumBase{TClass, TValue}.TryFromName(string, bool, out TClass)"/>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool TryFromName(string? name, out TClass? result) =>
        TryFromName(name, false, out result);

    /// <summary>
    /// Gets the item associated with the specified name.
    /// </summary>
    /// <param name="name">The name of the item to get.</param>
    /// <param name="ignoreCase"><c>true</c> to ignore case during the comparison; otherwise, <c>false</c>.</param>
    /// <param name="result">
    /// When this method returns, contains the item associated with the specified name, if the name is found; 
    /// otherwise, <c>null</c>. This parameter is passed uninitialized.</param>
    /// <returns>
    /// <c>true</c> if the <see cref="StrongEnumBase{TClass, TValue}"/> contains an item with the specified name; otherwise, <c>false</c>.
    /// </returns>
    /// <exception cref="ArgumentException"><paramref name="name"/> is <c>null</c>.</exception> 
    /// <seealso cref="StrongEnumBase{TClass, TValue}.FromName(string, bool)"/>
    /// <seealso cref="StrongEnumBase{TClass, TValue}.TryFromName(string, out TClass)"/>
    public static bool TryFromName(string? name, bool ignoreCase, out TClass? result)
    {
        if (string.IsNullOrEmpty(name))
        {
            result = default;
            return false;
        }

        return ignoreCase ?
            _optionFromNameIgnoreCase.Value.TryGetValue(name, out result) :
            _optionFromName.Value.TryGetValue(name, out result);
    }

    // /// <summary>
    // /// Gets an item associated with the specified value.
    // /// </summary>
    // /// <param name="value">The value of the item to get.</param>
    // /// <returns>
    // /// The first item found that is associated with the specified value.
    // /// If the specified value is not found, throws a <see cref="KeyNotFoundException"/>.
    // /// </returns>
    // /// <exception cref="StrongEnumNotFoundException"><paramref name="value"/> does not exist.</exception> 
    // /// <seealso cref="StrongEnumBase{TClass, TValue}.FromValue(TValue, TClass)"/>
    // /// <seealso cref="StrongEnumBase{TClass, TValue}.TryFromValue(TValue, out TClass)"/>
    // public static TClass FromValue(TValue? value)
    // {
    //     //value is null || 
    //     if (!_optionFromValue.Value.TryGetValue(value, out var result))
    //     {
    //         throw StrongEnumNotFoundException.ThrowByValue<TClass, TValue>(value);
    //     }
    //
    //     return result;
    // }
    //
    // /// <summary>
    // /// Gets an item associated with the specified value.
    // /// </summary>
    // /// <param name="value">The value of the item to get.</param>
    // /// <param name="defaultValue">The value to return when item not found.</param>
    // /// <returns>
    // /// The first item found that is associated with the specified value.
    // /// If the specified value is not found, returns <paramref name="defaultValue"/>.
    // /// </returns>
    // /// <seealso cref="StrongEnumBase{TClass, TValue}.FromValue(TValue)"/>
    // /// <seealso cref="StrongEnumBase{TClass, TValue}.TryFromValue(TValue, out TClass)"/>
    // public static TClass FromValue(TValue? value, TClass defaultValue)
    // {
    //     if (value is null)
    //     {
    //         throw StrongEnumNotFoundException.ThrowByValue<TClass, TValue>(value);
    //     }
    //     
    //     if (value == null)
    //         ThrowHelper.ThrowArgumentNullException(nameof(value));
    //
    //     if (!_fromValue.Value.TryGetValue(value, out var result))
    //     {
    //         return defaultValue;
    //     }
    //     return result;
    // }
    //
    // /// <summary>
    // /// Gets an item associated with the specified value.
    // /// </summary>
    // /// <param name="value">The value of the item to get.</param>
    // /// <param name="result">
    // /// When this method returns, contains the item associated with the specified value, if the value is found; 
    // /// otherwise, <c>null</c>. This parameter is passed uninitialized.</param>
    // /// <returns>
    // /// <c>true</c> if the <see cref="StrongEnumBase{TClass, TValue}"/> contains an item with the specified name; otherwise, <c>false</c>.
    // /// </returns>
    // /// <seealso cref="StrongEnumBase{TClass, TValue}.FromValue(TValue)"/>
    // /// <seealso cref="StrongEnumBase{TClass, TValue}.FromValue(TValue, TClass)"/>
    // public static bool TryFromValue(TValue value, out TClass result)
    // {
    //     if (value == null)
    //     {
    //         result = default;
    //         return false;
    //     }
    //
    //     return _fromValue.Value.TryGetValue(value, out result);
    // }
    //
    // public override string ToString() =>
    //     _name;
    //
    // [MethodImpl(MethodImplOptions.AggressiveInlining)]
    // public override int GetHashCode() =>
    //     _value.GetHashCode();
    //
    // public override bool Equals(object obj) =>
    //     (obj is StrongEnumBase<TClass, TValue> other) && Equals(other);
    //
    // /// <summary>
    // /// Returns a value indicating whether this instance is equal to a specified <see cref="StrongEnumBase{TClass, TValue}"/> value.
    // /// </summary>
    // /// <param name="other">An <see cref="StrongEnumBase{TClass, TValue}"/> value to compare to this instance.</param>
    // /// <returns><c>true</c> if <paramref name="other"/> has the same value as this instance; otherwise, <c>false</c>.</returns>
    // public virtual bool Equals(StrongEnumBase<TClass, TValue> other)
    // {
    //     // check if same instance
    //     if (Object.ReferenceEquals(this, other))
    //         return true;
    //
    //     // it's not same instance so 
    //     // check if it's not null and is same value
    //     if (other is null)
    //         return false;
    //
    //     return _value.Equals(other._value);
    // }
    //
    // /// <summary>
    // /// When this instance is one of the specified <see cref="StrongEnumBase{TClass, TValue}"/> parameters.
    // /// Execute the action in the subsequent call to Then().
    // /// </summary>
    // /// <param name="smartEnumWhen">A collection of <see cref="StrongEnumBase{TClass, TValue}"/> values to compare to this instance.</param>
    // /// <returns>A executor object to execute a supplied action.</returns>
    // public StrongEnumBaseThen<TClass, TValue> When(StrongEnumBase<TClass, TValue> smartEnumWhen) =>
    //     new StrongEnumBaseThen<TClass, TValue>(this.Equals(smartEnumWhen), false, this);
    //
    // /// <summary>
    // /// When this instance is one of the specified <see cref="StrongEnumBase{TClass, TValue}"/> parameters.
    // /// Execute the action in the subsequent call to Then().
    // /// </summary>
    // /// <param name="smartEnums">A collection of <see cref="StrongEnumBase{TClass, TValue}"/> values to compare to this instance.</param>
    // /// <returns>A executor object to execute a supplied action.</returns>
    // public StrongEnumBaseThen<TClass, TValue> When(params StrongEnumBase<TClass, TValue>[] smartEnums) =>
    //     new StrongEnumBaseThen<TClass, TValue>(smartEnums.Contains(this), false, this);
    //
    // /// <summary>
    // /// When this instance is one of the specified <see cref="StrongEnumBase{TClass, TValue}"/> parameters.
    // /// Execute the action in the subsequent call to Then().
    // /// </summary>
    // /// <param name="smartEnums">A collection of <see cref="StrongEnumBase{TClass, TValue}"/> values to compare to this instance.</param>
    // /// <returns>A executor object to execute a supplied action.</returns>
    // public StrongEnumBaseThen<TClass, TValue> When(IEnumerable<StrongEnumBase<TClass, TValue>> smartEnums) =>
    //     new StrongEnumBaseThen<TClass, TValue>(smartEnums.Contains(this), false, this);
    //
    // public static bool operator ==(StrongEnumBase<TClass, TValue> left, StrongEnumBase<TClass, TValue> right)
    // {
    //     // Handle null on left side
    //     if (left is null)
    //         return right is null; // null == null = true
    //
    //     // Equals handles null on right side
    //     return left.Equals(right);
    // }
    //
    // [MethodImpl(MethodImplOptions.AggressiveInlining)]
    // public static bool operator !=(StrongEnumBase<TClass, TValue> left, StrongEnumBase<TClass, TValue> right) =>
    //     !(left == right);
    //
    // /// <summary>
    // /// Compares this instance to a specified <see cref="StrongEnumBase{TClass, TValue}"/> and returns an indication of their relative values.
    // /// </summary>
    // /// <param name="other">An <see cref="StrongEnumBase{TClass, TValue}"/> value to compare to this instance.</param>
    // /// <returns>A signed number indicating the relative values of this instance and <paramref name="other"/>.</returns>
    // [MethodImpl(MethodImplOptions.AggressiveInlining)]
    // public virtual int CompareTo(StrongEnumBase<TClass, TValue> other) =>
    //     _value.CompareTo(other._value);
    //
    // [MethodImpl(MethodImplOptions.AggressiveInlining)]
    // public static bool operator <(StrongEnumBase<TClass, TValue> left, StrongEnumBase<TClass, TValue> right) =>
    //     left.CompareTo(right) < 0;
    //
    // [MethodImpl(MethodImplOptions.AggressiveInlining)]
    // public static bool operator <=(StrongEnumBase<TClass, TValue> left, StrongEnumBase<TClass, TValue> right) =>
    //     left.CompareTo(right) <= 0;
    //
    // [MethodImpl(MethodImplOptions.AggressiveInlining)]
    // public static bool operator >(StrongEnumBase<TClass, TValue> left, StrongEnumBase<TClass, TValue> right) =>
    //     left.CompareTo(right) > 0;
    //
    // [MethodImpl(MethodImplOptions.AggressiveInlining)]
    // public static bool operator >=(StrongEnumBase<TClass, TValue> left, StrongEnumBase<TClass, TValue> right) =>
    //     left.CompareTo(right) >= 0;
    //
    // [MethodImpl(MethodImplOptions.AggressiveInlining)]
    // public static implicit operator TValue(StrongEnumBase<TClass, TValue> smartEnum) =>
    //     smartEnum is not null
    //         ? smartEnum._value 
    //         : default;
    //
    // [MethodImpl(MethodImplOptions.AggressiveInlining)]
    // public static explicit operator StrongEnumBase<TClass, TValue>(TValue value) =>
    //     FromValue(value);
    //
    //
    
    
    
    
    
    
    private static TClass[] GetAllOptions()
    {
        var baseType = typeof(TClass);
        return Assembly.GetAssembly(baseType)?
                   .GetTypes()
                   .Where(t => baseType.IsAssignableFrom(t))
                   .SelectMany(t => t.GetFieldsOfType<TClass>())
                   .OrderBy(t => t.Name)
                   .ToArray() 
               ?? Array.Empty<TClass>();
    }
    
    private static readonly Lazy<TClass[]> _allOptions = 
        new(GetAllOptions, LazyThreadSafetyMode.ExecutionAndPublication);
    
    private static readonly Lazy<Dictionary<string, TClass>> _optionFromName =
        new(() => _allOptions.Value.ToDictionary(item => item.Name));

    private static readonly Lazy<Dictionary<string, TClass>> _optionFromNameIgnoreCase =
        new(() => _allOptions.Value.ToDictionary(item => item.Name, StringComparer.OrdinalIgnoreCase));
 
    private static readonly Lazy<Dictionary<TValue, TClass>> _optionFromValue =
        new(() =>
        {
            // multiple enums with same value are allowed but store only one per value
            var dictionary = new Dictionary<TValue, TClass>();
            foreach (var item in _allOptions.Value)
            {
                dictionary.TryAdd(item.Value, item);
            }
            return dictionary;
        });

    
    
    
}
//
// public abstract class StrongEnumBase<TClass, TValue, TData> 
//     : StrongEnumBase<TClass, TValue>
//     where TClass : class 
//     where TValue : IConvertible
//     where TData : notnull
// {
//     /// <summary>
//     /// Data property.
//     /// </summary>
//     /// <value>A <typeparamref name="TData"/> that is the data of the <see cref="StrongEnumBase{TClass, TValue, TData}"/>.</value>
//     public TData Data { get; private init; }
//     
//     protected StrongEnumBase(string name, TValue value, TData data)
//         : base(name, value)
//     {
//         Data = data;
//     }
// }
//
// public abstract class StrongEnumBase<TClass> 
//     : StrongEnumBase<TClass, int>
//     where TClass : class 
// {
//     protected StrongEnumBase(string name, int value)
//         : base(name, value)
//     {
//     }
// }