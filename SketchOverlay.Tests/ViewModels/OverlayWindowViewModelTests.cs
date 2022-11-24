using CommunityToolkit.Mvvm.Messaging;
using Moq;
using SketchOverlay.Drawing.Canvas;
using SketchOverlay.Drawing.Tools;
using SketchOverlay.Messages;
using SketchOverlay.Messages.Actions;
using SketchOverlay.Models;
using SketchOverlay.ViewModels;
using SUT = SketchOverlay.ViewModels.OverlayWindowViewModel;

namespace SketchOverlay.Tests.ViewModels;


// TODO: Draw actions call canvas draw methods.
public class OverlayWindowViewModelTests
{
    private static readonly IMessenger TestMessenger = Globals.Messenger;
    private readonly Mock<IDrawingCanvas> _mockCanvas;
    private readonly SUT _sut;

    public OverlayWindowViewModelTests()
    {
        _mockCanvas = new Mock<IDrawingCanvas>();
        _mockCanvas.SetupAllProperties();
        _sut = new SUT(_mockCanvas.Object, TestMessenger);
    }

    [Fact]
    public void MessengerRegistered()
    {
        Assert.True(TestMessenger.IsRegistered<DrawingWindowPropertyChangedMessage>(_sut));
        Assert.True(TestMessenger.IsRegistered<OverlayWindowCanvasActionMessage>(_sut));
        Assert.True(TestMessenger.IsRegistered<OverlayWindowDrawActionMessage>(_sut));
        Assert.True(TestMessenger.IsRegistered<OverlayWindowCancelDrawingMessage>(_sut));
    }

    [Fact]
    public void Receive_UndoMessage_CallsCanvasUndo()
    {
        // Act
        _sut.Receive(new OverlayWindowCanvasActionMessage(CanvasAction.Undo));

        // Assert
        _mockCanvas.Verify(x => x.Undo(), Times.Once);
    }

    [Fact]
    public void Receive_RedoMessage_CallsCanvasRedo()
    {
        // Act
        _sut.Receive(new OverlayWindowCanvasActionMessage(CanvasAction.Redo));

        // Assert
        _mockCanvas.Verify(x => x.Redo(), Times.Once);
    }

    [Fact]
    public void Receive_ClearMessage_CallsCanvasClear()
    {
        // Act
        _sut.Receive(new OverlayWindowCanvasActionMessage(CanvasAction.Clear));

        // Assert
        _mockCanvas.Verify(x => x.Clear(), Times.Once);
    }

    [Fact]
    public void Receive_CancelDrawingMessage_CancelsDrawing()
    {
        // Act
        _sut.Receive(new OverlayWindowCancelDrawingMessage());

        // Assert
        _mockCanvas.Verify(x=> x.CancelDrawingEvent(), Times.Once);
    }

    [Fact]
    public void Receive_PropertyChangedMessage_SetsCanvasStrokeColor()
    {
        // Arrange
        DrawingWindowPropertyChangedMessage message = new(Colors.Bisque, 
            nameof(DrawingToolWindowViewModel.SelectedDrawingColor));

        // Act
        _sut.Receive(message);

        // Assert
        Assert.Equal(message.Value, _mockCanvas.Object.StrokeColor);
        _mockCanvas.Verify(x => x.StrokeColor, Times.Once);
    }

    [Fact]
    public void Receive_PropertyChangedMessage_SetsCanvasDrawingTool()
    {
        // Arrange
        DrawingWindowPropertyChangedMessage message = new(
            new DrawingToolInfo(new RectangleTool(), "icon", "name"),
            nameof(DrawingToolWindowViewModel.SelectedDrawingTool));

        // Act
        _sut.Receive(message);

        // Assert
        Assert.Equal(((DrawingToolInfo)message.Value).Tool, _mockCanvas.Object.DrawingTool);
        _mockCanvas.Verify(x => x.DrawingTool, Times.Once);
    }

    [Fact]
    public void Receive_PropertyChangedMessage_SetsCanvasStrokeSize()
    {
        // Arrange
        DrawingWindowPropertyChangedMessage message = new(10,
            nameof(DrawingToolWindowViewModel.SelectedDrawingSize));

        // Act
        _sut.Receive(message);

        // Assert
        Assert.Equal(Convert.ToSingle(message.Value), _mockCanvas.Object.StrokeSize);
        _mockCanvas.Verify(x => x.StrokeSize, Times.Once);
    }
}
