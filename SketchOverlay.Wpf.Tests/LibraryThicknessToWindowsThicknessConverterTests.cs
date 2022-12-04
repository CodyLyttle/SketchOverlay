using System.Globalization;
using System.Windows;
using SketchOverlay.Library.Models;
using SketchOverlay.Wpf.BindingConverters;

namespace SketchOverlay.Wpf.Tests;

public class LibraryThicknessToWindowsThicknessConverterTests
{
    private readonly LibraryThicknessToWindowsThicknessConverter _sut;

    public LibraryThicknessToWindowsThicknessConverterTests()
    {
        _sut = new LibraryThicknessToWindowsThicknessConverter();
    }

    [Fact]
    public void Convert_ConvertsLibraryThicknessToEquivalentWindowsThickness()
    {
        // Arrange
        LibraryThickness libraryThickness = new(-22, -11, 11, 22);

        // Act
        var windowsThickness = (Thickness)_sut.Convert(
            libraryThickness, null!, null!, null!);

        // Assert
        AssertEquivalentThickness(libraryThickness, windowsThickness);
    }

    [Fact]
    public void ConvertBack_ConvertsWindowsThicknessToEquivalentLibraryThickness()
    {
        // Arrange
        Thickness windowsThickness = new(-22, -11, 11, 22);

        // Act
        var libraryThickness = (LibraryThickness)_sut.ConvertBack(
            windowsThickness, null!, null!, null!);

        // Assert
        AssertEquivalentThickness(libraryThickness, windowsThickness);
    }

    private static void AssertEquivalentThickness(LibraryThickness libraryThickness, Thickness thickness)
    {
        Assert.Equal(libraryThickness.Left, thickness.Left);
        Assert.Equal(libraryThickness.Top, thickness.Top);
        Assert.Equal(libraryThickness.Right, thickness.Right);
        Assert.Equal(libraryThickness.Bottom, thickness.Bottom);
    }
}