namespace SketchOverlay.Library.Tests.TestHelpers;

internal static class PropertyAssertionExtensions
{
    public static void AssertSetsValue<TValue>(this object instance, string propertyName, TValue value)
    {
        PropertyAssertions.SetsInputValue(instance, propertyName, value);
    }
}