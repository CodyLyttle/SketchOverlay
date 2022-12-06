using Moq;
using SketchOverlay.Library.Drawing;
using SketchOverlay.Library.Tests.TestHelpers;
using SUT = SketchOverlay.Library.Drawing.Canvas.CanvasProperties<System.Guid>;

namespace SketchOverlay.Library.Tests;

public class CanvasPropertiesTests
{
    #region Setups

    private readonly SUT _sut;
    private readonly IColorPalette<Guid> _colorPalette;

    public CanvasPropertiesTests()
    {
        Mock<IColorPalette<Guid>> mockCanvasProps = new();
        mockCanvasProps.Setup(x => x.DefaultPrimaryColor).Returns(Guid.NewGuid());
        mockCanvasProps.Setup(x => x.DefaultSecondaryColor).Returns(Guid.NewGuid());
        mockCanvasProps.Setup(x => x.PrimaryColor).Returns(Guid.NewGuid());
        mockCanvasProps.Setup(x => x.SecondaryColor).Returns(Guid.NewGuid());

        _colorPalette = mockCanvasProps.Object;
        _sut = new SUT(_colorPalette);
    }

    #endregion

    #region Properties

    [Fact]
    public void DefaultFillColor_OnInitialized_EqualToColorPaletteDefaultSecondaryColor()
    {
        Assert.Equal(_colorPalette.DefaultSecondaryColor, _sut.DefaultFillColor);
    }

    [Fact]
    public void DefaultStrokeColor_OnInitialized_EqualToColorPaletteDefaultPrimaryColor()
    {
        Assert.Equal(_colorPalette.DefaultPrimaryColor, _sut.DefaultStrokeColor);
    }

    [Fact]
    public void FillColor_OnInitialized_EqualToColorPaletteSecondaryColor()
    {
        Assert.Equal(_colorPalette.SecondaryColor, _sut.FillColor);
    }

    [Fact]
    public void StrokeColor_OnInitialized_EqualToColorPalettePrimaryColor()
    {
        Assert.Equal(_colorPalette.PrimaryColor, _sut.StrokeColor);
    }

    [Fact]
    public void StrokeSize_OnInitialized_EqualsDefaultStrokeSize()
    {
        Assert.Equal(_sut.DefaultStrokeSize, _sut.StrokeSize);
    }

    [Fact]
    public void FillColor_Setter_SetsInputValue()
    {
        PropertyAssertions.SetsInputValue(_sut,
            nameof(SUT.FillColor),
            Guid.NewGuid());
    }

    [Fact]
    public void StrokeColor_Setter_SetsInputValue()
    {
        PropertyAssertions.SetsInputValue(_sut,
            nameof(SUT.StrokeColor),
            Guid.NewGuid());
    }

    [Fact]
    public void StrokeSize_Setter_SetsInputValue()
    {
        PropertyAssertions.SetsInputValue(_sut, 
            nameof(SUT.StrokeSize), 
            (float)Random.Shared.Next());
    }

    #endregion
}