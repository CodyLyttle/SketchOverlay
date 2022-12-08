namespace SketchOverlay.Library.Tests;

public class ValueConverterTypeExceptionTests
{
    [Fact]
    public void ExceptionMessage_ContainsInformationAboutTypeMismatch()
    {
        // Arrange
        const string expectedMessage = "Expected type \"String\", actual type \"Single\"";
        ValueConverterTypeException<string> exception = new(123f);

        // Assert
        Assert.Equal(expectedMessage, exception.Message);
    }
}