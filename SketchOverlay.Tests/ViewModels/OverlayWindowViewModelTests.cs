using CommunityToolkit.Mvvm.Messaging;
using Moq;
using SketchOverlay.Drawing.Canvas;
using SketchOverlay.Drawing.Tools;
using SketchOverlay.Messages;
using SketchOverlay.ViewModels;

namespace SketchOverlay.Tests.ViewModels;

public class OverlayWindowViewModelTests
{
    private static readonly IMessenger TestMessenger = WeakReferenceMessenger.Default;
    private readonly Mock<IDrawingCanvas> _mockCanvas;
    private readonly OverlayWindowViewModel _sut;

    public OverlayWindowViewModelTests()
    {
        _mockCanvas = new Mock<IDrawingCanvas>();
        _mockCanvas.SetupAllProperties();
        _sut = new OverlayWindowViewModel(_mockCanvas.Object);
    }

    [Fact]
    public void Receive_WithUndoMessage_CallsCanvasUndo()
    {
        // Arrange
        CanvasActionMessage message = new(CanvasAction.Undo);

        // Act
        _sut.Receive(message);
        TestMessenger.Send(message);

        // Assert
        _mockCanvas.Verify(x => x.Undo(), Times.Exactly(2));
        _mockCanvas.Verify(x => x.Redo(), Times.Never);
        _mockCanvas.Verify(x => x.Clear(), Times.Never);
    }

    [Fact]
    public void Receive_WithRedoMessage_CallsCanvasRedo()
    {
        // Arrange
        CanvasActionMessage message = new(CanvasAction.Redo);

        // Act
        _sut.Receive(message);
        TestMessenger.Send(message);

        // Assert
        _mockCanvas.Verify(x => x.Undo(), Times.Never);
        _mockCanvas.Verify(x => x.Redo(), Times.Exactly(2));
        _mockCanvas.Verify(x => x.Clear(), Times.Never);
    }

    [Fact]
    public void Receive_WithClearMessage_CallsCanvasClear()
    {
        // Arrange
        CanvasActionMessage message = new(CanvasAction.Clear);

        // Act
        _sut.Receive(message);
        TestMessenger.Send(message);

        // Assert
        _mockCanvas.Verify(x => x.Undo(), Times.Never);
        _mockCanvas.Verify(x => x.Redo(), Times.Never);
        _mockCanvas.Verify(x => x.Clear(), Times.Exactly(2));
    }

    [Fact]
    public void Receive_WithDrawingColorChangedMessage_SetsCanvasStrokeColor()
    {
        // Arrange
        DrawingColorChangedMessage messageA = new(Colors.Bisque);
        DrawingColorChangedMessage messageB = new(Colors.BurlyWood);

        // Act
        _sut.Receive(messageA);
        Color actualA = _sut.Canvas.StrokeColor;

        TestMessenger.Send(messageB);
        Color actualB = _sut.Canvas.StrokeColor;

        // Assert
        Assert.Equal(messageA.Value, actualA);
        Assert.Equal(messageB.Value, actualB);
        _mockCanvas.Verify(x=> x.StrokeColor, Times.Exactly(2));
    }

    [Fact]
    public void Receive_WithDrawingToolChangedMessage_SetsCanvasDrawingTool()
    {
        // Arrange
        DrawingToolChangedMessage messageA = new(new BrushTool());
        DrawingToolChangedMessage messageB = new(new LineTool());

        // Act
        _sut.Receive(messageA);
        IDrawingTool actualA = _sut.Canvas.DrawingTool;

        TestMessenger.Send(messageB);
        IDrawingTool actualB = _sut.Canvas.DrawingTool;

        // Assert
        Assert.Equal(messageA.Value, actualA);
        Assert.Equal(messageB.Value, actualB);
        _mockCanvas.Verify(x => x.DrawingTool, Times.Exactly(2));
    }
}
