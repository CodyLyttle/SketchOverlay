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
        Assert.False((bool)_sut.Convert(true, null!, null!, null!));
        Assert.True((bool)_sut.Convert(false, null!, null!, null!));
    }

    [Fact]
    public void ConvertBack_InvertsValue()
    {
        Assert.False((bool)_sut.Convert(true, null!, null!, null!));
        Assert.True((bool)_sut.Convert(false, null!, null!, null!));
    }
}