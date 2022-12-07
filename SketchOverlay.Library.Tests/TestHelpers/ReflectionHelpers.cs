using System.Reflection;

namespace SketchOverlay.Library.Tests.TestHelpers;

internal static class ReflectionExtensions
{
    public static TValue GetField<TValue>(this object objectInstance, string fieldName)
    {
        Type type = objectInstance.GetType();
        FieldInfo? field = type.GetField(fieldName,
            BindingFlags.Public |
            BindingFlags.NonPublic |
            BindingFlags.Instance);

        if (field is null)
            throw new ArgumentOutOfRangeException(nameof(fieldName), $"Field with name {fieldName} doesn't exist");

        return (TValue)field.GetValue(objectInstance)!;
    }

    public static void SetField<TValue>(this object objectInstance, string fieldName, TValue value)
    {
        Type type = objectInstance.GetType();
        FieldInfo? field = type.GetField(fieldName,
            BindingFlags.Public |
            BindingFlags.NonPublic |
            BindingFlags.Instance);

        if (field is null)
            throw new ArgumentOutOfRangeException(nameof(fieldName), $"Field with name {fieldName} doesn't exist");

        field.SetValue(objectInstance, value);
    }

    public static TValue GetPropertyValue<TValue>(this object objectInstance, string propertyName)
    {
        return (TValue)objectInstance.GetType()
            .GetProperty(propertyName)!
            .GetValue(objectInstance)!;
    }

    public static void SetPropertyValue<TValue>(this object instance, string propertyName, TValue value)
    {
        PropertyInfo? propInfo = instance.GetType()
            .GetProperty(propertyName);

        while (propInfo is not null)
        {
            if (propInfo.CanWrite)
            {
                propInfo.SetValue(instance, value);
                return;
            }

            // Setter not found in current class, check base class if exists.
            // Private base class setters cannot be retrieved from a derived class.
            propInfo = propInfo.DeclaringType?
                .GetProperty(propertyName);
        }

        throw new InvalidOperationException($"Setter doesn't exist");
    }
    public static void ThrowIfMatchingPropertyValue<TValue>(this object objectInstance, string propertyName, TValue value)
    {
        ThrowIfMatchingPropertyValue(objectInstance, propertyName, value, out TValue _);
    }

    public static void ThrowIfMatchingPropertyValue<TValue>(this object objectInstance, string propertyName, TValue value, out TValue currentValue)
    {
        currentValue = objectInstance.GetPropertyValue<TValue>(propertyName);

        if (EqualityComparer<TValue>.Default.Equals(currentValue, value))
            throw new ArgumentOutOfRangeException(nameof(value),
                $"The {nameof(value)} argument matches the current value of the property");
    }
}