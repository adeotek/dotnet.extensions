using System.Reflection;

namespace Adeotek.StrongEnums;

public static class Extensions
{
    public static List<TFieldType> GetFieldsOfType<TFieldType>(this Type type)
    {
        return type.GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy)
            .Where(p => type.IsAssignableFrom(p.FieldType))
            .Select(pi => (TFieldType)pi.GetValue(null)!)
            .ToList();
    }
}