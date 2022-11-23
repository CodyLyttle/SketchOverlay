using CommunityToolkit.Mvvm.Messaging;
using Moq;
using SketchOverlay.Drawing.Canvas;
using SketchOverlay.Drawing.Tools;
using SketchOverlay.Messages;
using SketchOverlay.Messages.Actions;
using SketchOverlay.ViewModels;

namespace SketchOverlay.Tests.ViewModels;

public class OverlayWindowViewModelTests
{
    private static readonly IMessenger TestMessenger = Globals.Messenger;
    private readonly Mock<IDrawingCanvas> _mockCanvas;
    private readonly OverlayWindowViewModel _sut;

    public OverlayWindowViewModelTests()
    {
        _mockCanvas = new Mock<IDrawingCanvas>();
        _mockCanvas.SetupAllProperties();
        _sut = new OverlayWindowViewModel(_mockCanvas.Object, TestMessenger);
    }

    [Fact]
    public void MessengerRegistered()
    {
        Assert.True(TestMessenger.IsRegistered<RequestCanvasActionMessage>(_sut));
        Assert.True(TestMessenger.IsRegistered<DrawingColorChangedMessage>(_sut));
        Assert.True(TestMessenger.IsRegistered<DrawingToolChangedMessage>(_sut));
        Assert.True(TestMessenger.IsRegistered<DrawingSizeChangedMessage>(_sut));
    }

    [Fact]
    public void Receive_UndoMessage_CallsCanvasUndo()
    {
        // Act
        _sut.Receive(new RequestCanvasActionMessage(CanvasAction.Undo));

        // Assert
        _mockCanvas.Verify(x => x.Undo(), Times.Once);
    }

    [Fact]
    public void Receive_RedoMessage_CallsCanvasRedo()
    {
        // Act
        _sut.Receive(new RequestCanvasActionMessage(CanvasAction.Redo));

        // Assert
        _mockCanvas.Verify(x => x.Redo(), Times.Once);
    }

    [Fact]
    public void Receive_ClearMessage_CallsCanvasClear()
    {
        // Act
        _sut.Receive(new RequestCanvasActionMessage(CanvasAction.Clear));

        // Assert
        _mockCanvas.Verify(x => x.Clear(), Times.Once);
    }

    [Fact]
    public void Receive_DrawingColorChangedMessage_SetsCanvasStrokeColor()
    {
        // Arrange
        DrawingColorChangedMessage message = new(Colors.Bisque);

        // Act
        _sut.Receive(message);

        // Assert
        Assert.Equal(message.Value, _sut.Canvas.StrokeColor);
        _mockCanvas.Verify(x=> x.StrokeColor, Times.Once);
    }

    [Fact]
    public void Receive_DrawingToolChangedMessage_SetsCanvasDrawingTool()
    {
        // Arrange
        DrawingToolChangedMessage message = new(new BrushTool());

        // Act
        _sut.Receive(message);

        // Assert
        Assert.Equal(message.Value, _sut.Canvas.DrawingTool);
        _mockCanvas.Verify(x => x.DrawingTool, Times.Once);
    }

    [Fact]
    public void Receive_DrawingSizeChangedMessage_SetsCanvasStrokeSize()
    {
        // Arrange
        DrawingSizeChangedMessage message = new(10);

        // Act
        _sut.Receive(message);

        // Assert
        Assert.Equal(message.Value, _sut.Canvas.StrokeSize);
        _mockCanvas.Verify(x => x.StrokeSize, Times.Once);
    }
}
