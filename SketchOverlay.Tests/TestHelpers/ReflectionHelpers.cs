using System.Net.Http.Headers;

namespace SketchOverlay.Tests.TestHelpers;

internal static class ReflectionExtensions
{
    public static object GetPropertyValue(this object objectInstance, string propertyName)
    {
        return objectInstance.GetType()
            .GetProperty(propertyName)!
            .GetValue(objectInstance)!;
    }

    public static TValue GetPropertyValue<TValue>(this object objectInstance, string propertyName)
    {
        return (TValue)objectInstance.GetType()
            .GetProperty(propertyName)!
            .GetValue(objectInstance)!;
    }

    public static void SetPropertyValue<TValue>(this object objectInstance, string propertyName, TValue value)
    {
        objectInstance.GetType()
            .GetProperty(propertyName)!
            .SetValue(objectInstance, value);
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