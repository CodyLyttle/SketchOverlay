using CommunityToolkit.Mvvm.Messaging;
using Moq;
using SketchOverlay.Drawing.Canvas;
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
}
