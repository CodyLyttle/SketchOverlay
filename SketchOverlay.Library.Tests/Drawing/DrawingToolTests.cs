using System.Drawing;
using Moq;
using Moq.Protected;
using SketchOverlay.Library.Drawing.Canvas;
using SketchOverlay.Library.Tests.TestHelpers;
using SUT = SketchOverlay.Library.Drawing.Tools.DrawingTool<object, object>;

namespace SketchOverlay.Library.Tests.Drawing;

public class DrawingToolTests
{
    #region Setups

    private readonly SUT _sut;
    private readonly Mock<SUT> _mockTool;
    private readonly ICanvasProperties<object> _canvasProps;

    public DrawingToolTests()
    {
        _canvasProps = new Mock<ICanvasProperties<object>>().Object;

        _mockTool = new Mock<SUT>();
        _mockTool.Protected().Setup("InitializeDrawingProperties", _canvasProps, It.IsAny<PointF>());
        _mockTool.Protected().Setup("DoUpdateDrawing", It.IsAny<PointF>());
        _mockTool.Protected().Setup("DoFinishDrawing");

        _sut = _mockTool.Object;
    }

    private void SetupCurrentDrawingIsNull()
    {
        _sut.SetPropertyValue<object?>(nameof(SUT.CurrentDrawing), null);
    }

    private void SetupCurrentDrawingNotNull()
    {
        _sut.SetPropertyValue(nameof(SUT.CurrentDrawing), new object());
    }

    #endregion

    #region CurrentDrawing

    [Fact]
    public void CurrentDrawing_OnInstantiation_IsNull()
    {
        Assert.Throws<InvalidOperationException>(() => _sut.CurrentDrawing);
    }

    [Fact]
    public void CurrentDrawing_IsNull_ThrowsInvalidOperationException()
    {
        // Arrange
        SetupCurrentDrawingIsNull();

        // Act/Assert
        Assert.Throws<InvalidOperationException>(() => _sut.CurrentDrawing);
    }

    #endregion

    #region CreateDrawing

    [Fact]
    public void CreateDrawing_CurrentDrawingNotNull_ThrowsInvalidOperationException()
    {
        // Arrange
        SetupCurrentDrawingNotNull();

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
        // Arrange
        SetupCurrentDrawingIsNull();

        // Act/Assert
        Assert.Throws<InvalidOperationException>(() => _sut.UpdateDrawing(new PointF()));
    }

    [Fact]
    public void UpdateDrawing_CurrentDrawingNotNull_UpdatesDrawing()
    {
        // Arrange
        SetupCurrentDrawingNotNull();
        PointF expectedPoint = new(1, 2);

        // Act
        _sut.UpdateDrawing(expectedPoint);

        // Assert
        _mockTool.Protected().Verify("DoUpdateDrawing", Times.Once(),
            expectedPoint);
    }

    #endregion

    #region FinishDrawing

    [Fact]
    public void FinishDrawing_CurrentDrawingIsNull_ThrowsInvalidOperationException()
    {
        // Arrange
        SetupCurrentDrawingIsNull();

        // Act/Assert
        Assert.Throws<InvalidOperationException>(() => _sut.FinishDrawing());
    }

    [Fact]
    public void FinishDrawing_CurrentDrawingNotNull_FinishesDrawing()
    {
        // Arrange
        SetupCurrentDrawingNotNull();

        // Act
        _sut.FinishDrawing();

        // Assert
        _mockTool.Protected().Verify("DoFinishDrawing", Times.Once());
    }

    [Fact]
    public void FinishDrawing_CurrentDrawingNotNull_SetsCurrentDrawingToNull()
    {
        // Arrange
        SetupCurrentDrawingNotNull();

        // Act
        _sut.FinishDrawing();

        // Assert
        Assert.Throws<InvalidOperationException>(() => _sut.CurrentDrawing);
    }

    #endregion
}