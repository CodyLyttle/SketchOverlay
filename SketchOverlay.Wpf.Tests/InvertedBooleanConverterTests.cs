using System.Globalization;
using SketchOverlay.Wpf.BindingConverters;

namespace SketchOverlay.Wpf.Tests;

public class InvertedBooleanConverterTests
{
    private readonly InvertedBooleanConverter _sut;

    public InvertedBooleanConverterTests()
    {
        _sut = new InvertedBooleanConverter();
    }

    [Fact]
    public void Convert_InvertsValue()
    {
        Assert.False((bool)_sut.Convert(true, typeof(bool), null!, CultureInfo.CurrentCulture));
        Assert.True((bool)_sut.Convert(false, typeof(bool), null!, CultureInfo.CurrentCulture));
    }

    [Fact]
    public void ConvertBack_InvertsValue()
    {
        Assert.False((bool)_sut.Convert(true, typeof(bool), null!, CultureInfo.CurrentCulture));
        Assert.True((bool)_sut.Convert(false, typeof(bool), null!, CultureInfo.CurrentCulture));
    }
}