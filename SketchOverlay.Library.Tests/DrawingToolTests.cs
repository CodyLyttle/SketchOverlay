using System.Drawing;
using Moq;
using Moq.Protected;
using SketchOverlay.Library.Drawing.Canvas;
using SketchOverlay.Library.Tests.TestHelpers;
using SUT = SketchOverlay.Library.Drawing.Tools.DrawingTool<object, object>;

namespace SketchOverlay.Library.Tests;

public class DrawingToolTests
{
    private readonly SUT _sut;
    private readonly Mock<SUT> _mockTool;
    private readonly ICanvasProperties<object> _canvasProps;

    public DrawingToolTests()
    {
        _canvasProps = new Mock<ICanvasProperties<object>>().Object;

        _mockTool = new Mock<SUT>();
        _mockTool.Protected().Setup("InitializeDrawingProperties",
            _canvasProps, It.IsAny<PointF>());
        _mockTool.Protected().Setup("DoUpdateDrawing",
            It.IsAny<PointF>());

        _sut = _mockTool.Object;
    }

    [Fact]
    public void CurrentDrawing_IsNull_ThrowsInvalidOperationException()
    {
        // Arrange
        _sut.SetPropertyValue<object?>(nameof(SUT.CurrentDrawing), null);

        // Act/Assert
        Assert.Throws<InvalidOperationException>(() => _sut.CurrentDrawing);
    }

    [Fact]
    public void CurrentDrawing_OnInitialization_IsNull()
    {
        Assert.Throws<InvalidOperationException>(() => _sut.CurrentDrawing);
    }

    #region CreateDrawing

    [Fact]
    public void CreateDrawing_CurrentDrawingNotNull_ThrowsInvalidOperationException()
    {
        // Arrange
        _sut.SetPropertyValue("CurrentDrawing", new object());

        // Act/Assert
        Assert.Throws<InvalidOperationException>(() =>
            _sut.CreateDrawing(_canvasProps, new PointF()));
    }

    [Fact]
    public void CreateDrawing_CurrentDrawingIsNull_SetsCurrentDrawing()
    {
        // Act
        _sut.CreateDrawing(_canvasProps, new PointF());

        // Assert
        Assert.NotNull(_sut.CurrentDrawing);
    }

    [Fact]
    public void CreateDrawing_CurrentDrawingIsNull_ReturnsCurrentDrawing()
    {
        // Act
        object returnedDrawing = _sut.CreateDrawing(_canvasProps, new PointF());

        // Assert
        Assert.Equal(_sut.CurrentDrawing, returnedDrawing);
    }

    [Fact]
    public void CreateDrawing_CurrentDrawingIsNull_InitializeDrawingProperties()
    {
        // Arrange
        PointF expectedPoint = new(123, 321);

        // Act
        _sut.CreateDrawing(_canvasProps, expectedPoint);

        // Assert
        _mockTool.Protected().Verify("InitializeDrawingProperties", Times.Once(),
            _canvasProps, expectedPoint);
    }

    [Fact]
    public void CreateDrawing_CurrentDrawingIsNull_UpdatesDrawing()
    {
        // Arrange
        PointF expectedPoint = new(123, 321);

        // Act
        _sut.CreateDrawing(_canvasProps, expectedPoint);

        // Assert
        _mockTool.Protected().Verify("DoUpdateDrawing", Times.Once(),
            expectedPoint);
    }

    #endregion

    #region UpdateDrawing

    [Fact]
    public void UpdateDrawing_CurrentDrawingIsNull_ThrowsInvalidOperationException()
    {
        Assert.Throws<InvalidOperationException>(() => _sut.UpdateDrawing(new PointF()));
    }

    [Fact]
    public void UpdateDrawing_CurrentDrawingNotNull_UpdatesDrawing()
    {
        // Arrange
        _sut.SetPropertyValue(nameof(SUT.CurrentDrawing), new object());
        PointF expectedPoint = new(1,2);

        // Act
        _sut.UpdateDrawing(expectedPoint);

        // Assert
        _mockTool.Protected().Verify("DoUpdateDrawing", Times.Once(),
            expectedPoint);
    }

    #endregion
}