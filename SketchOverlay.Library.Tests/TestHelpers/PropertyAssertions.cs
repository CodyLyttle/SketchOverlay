namespace SketchOverlay.Library.Tests.TestHelpers;

internal static class PropertyAssertions
{
    public static void SetsInputValue<TValue>(object instance,
        string propertyName, TValue inputValue)
    {
        // Arrange
        instance.ThrowIfMatchingPropertyValue(propertyName, inputValue);

        // Act
        instance.SetPropertyValue(propertyName, inputValue);

        // Act
        Assert.Equal(inputValue, instance.GetPropertyValue<TValue>(propertyName));
    }
}