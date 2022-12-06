using Moq;
using SketchOverlay.Library.Drawing.Tools;
using SketchOverlay.Library.Models;
using SUT = SketchOverlay.Library.Drawing.Tools.DrawingToolCollection<object, object, object>;

namespace SketchOverlay.Library.Tests;

public class DrawingToolCollectionTests
{
    private readonly SUT _sut;
    private readonly DrawingToolInfo<object, object, object>[] _toolInfoObjects;

    public DrawingToolCollectionTests()
    {
        _toolInfoObjects = new[]
        {
            CreateRandomDrawingToolInfo(),
            CreateRandomDrawingToolInfo(),
            CreateRandomDrawingToolInfo()
        };

        _sut = new SUT(_toolInfoObjects);
    }

    private DrawingToolInfo<object, object, object> CreateRandomDrawingToolInfo()
    {
        return new DrawingToolInfo<object, object, object>(
            new Mock<IDrawingTool<object, object>>().Object,
            new object(),
            Guid.NewGuid().ToString());
    }

    #region IReadonlyCollection implementation.

    [Fact]
    public void Count_ReturnsToolCount()
    {
        Assert.Equal(_toolInfoObjects.Length, _sut.Count);
    }

    [Fact]
    public void Enumerator_EnumeratesToolCollection()
    {
        var index = 0;
        foreach (DrawingToolInfo<object, object, object> toolInfo in _sut)
        {
            Assert.Equal(_toolInfoObjects[index++], toolInfo);
        }
    }

    #endregion

    #region IDrawingToolRetriever implementation

    [Fact]
    public void DefaultTool_ReturnsFirstTool()
    {
        Assert.Equal(_toolInfoObjects[0].Tool, _sut.DefaultTool);
    }

    [Fact]
    public void SelectedTool_OnInitialization_ReturnsFirstTool()
    {
        Assert.Equal(_toolInfoObjects[0].Tool, _sut.SelectedTool);
    }

    #endregion

    #region IDrawingToolCollection implementation

    [Fact]
    public void SelectedToolInfo_OnInitialization_ReturnsFirstToolInfo()
    {
        Assert.Equal(_toolInfoObjects[0], _sut.SelectedToolInfo);
    }

    [Fact]
    public void GetTool_WithTypeOfToolNotFoundInCollection_ThrowsArgumentOutOfRangeException()
    {
        Assert.Throws<ArgumentOutOfRangeException>(_sut.GetTool<IPaintBrushTool<object, object>>);
    }

    [Fact]
    public void GetTool_WithTypeOfToolFoundInCollection_ReturnsFirstToolOfType()
    {
        // Act
        var result = _sut.GetTool<IDrawingTool<object, object>>();

        // Assert
        Assert.Equal(_toolInfoObjects[0].Tool, result);
    }

    #endregion
}