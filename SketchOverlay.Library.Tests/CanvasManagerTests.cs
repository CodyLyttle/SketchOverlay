using System.Drawing;
using SketchOverlay.Library.Drawing.Canvas;
using SketchOverlay.Library.Drawing.Tools;
using SketchOverlay.Library.Tests.TestHelpers;
using Moq;

namespace SketchOverlay.Library.Tests;
public class CanvasManagerTests
{
    private int _mockStackCount = 0;
    private readonly CanvasManager<object, object, object> _sut;
    private readonly Mock<ICanvasProperties<object>> _mockCanvasProps;
    private readonly Mock<IDrawingStack<object, object>> _mockDrawStack;
    private readonly Mock<IDrawingToolRetriever<object, object>> _mockToolRetriever;
    private readonly Mock<IDrawingTool<object, object>> _mockDrawingTool;

    public CanvasManagerTests()
    {
        _mockDrawStack = new Mock<IDrawingStack<object, object>>();
        _mockDrawStack.Setup(x => x.PushDrawing(It.IsAny<object>()))
            .Callback(() =>  _mockStackCount++);
        _mockDrawStack.Setup(x => x.Count)
            .Returns(() => _mockStackCount);

        _mockCanvasProps = new Mock<ICanvasProperties<object>>();
        _mockDrawingTool = new Mock<IDrawingTool<object, object>>();
        _mockToolRetriever = new Mock<IDrawingToolRetriever<object, object>>();
        _mockToolRetriever.Setup(x => x.SelectedTool).Returns(_mockDrawingTool.Object);

        _sut = new CanvasManager<object, object, object>(
            _mockCanvasProps.Object,
            _mockDrawStack.Object,
            _mockToolRetriever.Object);
    }

    private void ResetMockInvocations()
    {
        _mockCanvasProps.Invocations.Clear();
        _mockDrawStack.Invocations.Clear();
        _mockDrawingTool.Invocations.Clear();
        _mockToolRetriever.Invocations.Clear();
    }

    private EventCatcher<bool> CreateCanvasActionsEventCatcher()
    {
        EventCatcher<bool> canvasActionsCatcher = new();
        _sut.CanClearChanged += canvasActionsCatcher.OnReceived;
        _sut.CanRedoChanged += canvasActionsCatcher.OnReceived;
        _sut.CanUndoChanged += canvasActionsCatcher.OnReceived;

        return canvasActionsCatcher;
    }

    private void AssertNotDrawing()
    {
        Assert.False(_sut.IsDrawing);
    }

    private void SetupIsDrawing()
    {
        if (_sut.IsDrawing)
            return;

        _sut.DoDrawing(new PointF());
        ResetMockInvocations();
        Assert.True(_sut.IsDrawing);
    }

    #region DoDrawing_WhileNotDrawing

    [Fact]
    public void DoDrawing_WhileNotDrawing_SetsIsDrawingToTrue()
    {
        // Arrange
        AssertNotDrawing();

        // Act
        _sut.DoDrawing(new PointF());

        // Assert
        Assert.True(_sut.IsDrawing);
    }

    [Fact]
    public void DoDrawing_WhileNotDrawing_CreatesDrawingUsingCanvasProperties()
    {
        // Act
        AssertNotDrawing();
        PointF expectedPoint = new(123, 321);

        _sut.DoDrawing(expectedPoint);

        // Assert
        _mockDrawingTool.Verify(x => x.CreateDrawing(_mockCanvasProps.Object, expectedPoint));
        _mockDrawingTool.VerifyNoOtherCalls();
    }

    [Fact]
    public void DoDrawing_WhileNotDrawing_PushesDrawingToDrawStack()
    {
        // Act
        AssertNotDrawing();
        _sut.DoDrawing(new PointF());

        // Assert
        _mockDrawStack.Verify(x => x.PushDrawing(It.IsAny<object>()), Times.Once);
        _mockDrawStack.VerifyNoOtherCalls();
    }

    [Fact]
    public void DoDrawing_WhileNotDrawing_InvokesRequestsRedrawEvent()
    {
        // Arrange
        AssertNotDrawing();
        EventCatcher eventCatcher = new();
        _sut.RequestRedraw += eventCatcher.OnReceived;

        // Act
        _sut.DoDrawing(new PointF());

        // Assert
        Assert.Single(eventCatcher.Received);
    }

    [Fact]
    public void DoDrawing_WhileNotDrawing_DoesNotInvokeCanvasActionEvents()
    {
        // Arrange
        AssertNotDrawing();
        EventCatcher<bool> eventCatcher = CreateCanvasActionsEventCatcher();

        // Act
        _sut.DoDrawing(new PointF());

        // Assert
        Assert.Empty(eventCatcher.Received);
    }

    #endregion

    #region DoDrawing_WhileDrawing

    [Fact]
    public void DoDrawing_WhileDrawing_IsDrawingRemainsTrue()
    {
        // Arrange
        SetupIsDrawing();

        // Act
        _sut.DoDrawing(new PointF());

        // Assert
        Assert.True(_sut.IsDrawing);
    }

    [Fact]
    public void DoDrawing_WhileDrawing_DoesNotCreateDrawing()
    {
        // Arrange
        SetupIsDrawing();

        // Act
        _sut.DoDrawing(new PointF());

        // Assert
        _mockDrawingTool.Verify(x =>
            x.CreateDrawing(It.IsAny<ICanvasProperties<object>>(), It.IsAny<PointF>()),
            Times.Never);
    }

    [Fact]
    public void DoDrawing_WhileDrawing_DoesNotModifyDrawStack()
    {
        // Arrange
        SetupIsDrawing();

        // Act
        _sut.DoDrawing(new PointF());

        // Assert
        _mockDrawStack.VerifyNoOtherCalls();
    }

    [Fact]
    public void DoDrawing_WhileDrawing_UpdatesCurrentDrawingWithPointArgument()
    {
        // Arrange
        SetupIsDrawing();
        PointF expectedPoint = new(1, 2);

        // Act
        _sut.DoDrawing(expectedPoint);

        // Assert
        _mockDrawingTool.Verify(x => x.UpdateDrawing(expectedPoint), Times.Once);
        _mockDrawingTool.VerifyNoOtherCalls();
    }

    [Fact]
    public void DoDrawing_WhileDrawing_InvokesRequestRedrawEvent()
    {
        // Arrange
        SetupIsDrawing();
        EventCatcher eventCatcher = new();
        _sut.RequestRedraw += eventCatcher.OnReceived;

        // Act
        _sut.DoDrawing(new PointF());

        // Assert
        Assert.Single(eventCatcher.Received);
    }

    [Fact]
    public void DoDrawing_WhileDrawing_DoesNotInvokeCanvasActionEvents()
    {
        // Arrange
        SetupIsDrawing();
        EventCatcher<bool> eventCatcher = CreateCanvasActionsEventCatcher();

        // Act
        _sut.DoDrawing(new PointF());

        // Assert
        Assert.Empty(eventCatcher.Received);
    }

    #endregion
}