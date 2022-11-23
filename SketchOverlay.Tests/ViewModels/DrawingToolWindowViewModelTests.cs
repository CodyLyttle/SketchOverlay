using CommunityToolkit.Mvvm.Messaging;
using SketchOverlay.Drawing;
using SketchOverlay.Drawing.Tools;
using SketchOverlay.Messages;
using SketchOverlay.Messages.Actions;
using SketchOverlay.Models;
using SketchOverlay.ViewModels;

namespace SketchOverlay.Tests.ViewModels;

public class DrawingToolWindowViewModelTests
{
    private static readonly IMessenger TestMessenger = WeakReferenceMessenger.Default;
    private readonly DrawingToolWindowViewModel _sut;

    public DrawingToolWindowViewModelTests()
    {
        _sut = new DrawingToolWindowViewModel(TestMessenger);
    }

    [Fact]
    public void UndoCommand_SendsCanvasActionMessageWithValueUndo()
    {
        // Arrange
        const CanvasAction expected = CanvasAction.Undo;
        CanvasAction? actual = null;
        TestMessenger.Register<CanvasActionMessage>(this, (_, msg) => actual = msg.Value);

        // Act
        _sut.UndoCommand.Execute(null);

        // Assert
        Assert.Equal(expected, actual);
    }

    [Fact]
    public void RedoCommand_SendsCanvasActionMessageWithValueRedo()
    {
        // Arrange
        const CanvasAction expected = CanvasAction.Redo;
        CanvasAction? actual = null;
        TestMessenger.Register<CanvasActionMessage>(this, (_, msg) => actual = msg.Value);

        // Act
        _sut.RedoCommand.Execute(null);

        // Assert
        Assert.Equal(expected, actual);
    }

    [Fact]
    public void ClearCommand_SendsCanvasActionMessageWithValueClear()
    {
        // Arrange
        const CanvasAction expected = CanvasAction.Clear;
        CanvasAction? actual = null;
        TestMessenger.Register<CanvasActionMessage>(this, (_, msg) => actual = msg.Value);

        // Act
        _sut.ClearCommand.Execute(null);

        // Assert
        Assert.Equal(expected, actual);
    }

    [Fact]
    public void IsVisible_ValueChanged_SendsDrawingWindowVisibilityChangedMessage()
    {
        // Arrange
        bool expected = !_sut.IsVisible;
        var actual = false;

        // Act
        TestMessenger.Register<DrawingWindowVisibilityChangedMessage>(this, (_, msg) => actual = msg.Value);
        _sut.IsVisible = !_sut.IsVisible;

        // Assert
        Assert.Equal(expected, actual);
    }

    [Fact]
    public void IsVisible_SetWithExistingValue_SendsDrawingWindowVisibilityChangedMessage()
    {
        // Arrange
        var wasMessageReceived = false;

        // Act
        TestMessenger.Register<DrawingWindowVisibilityChangedMessage>(this, (_, _) => wasMessageReceived = true);
        _sut.IsVisible = _sut.IsVisible;

        // Assert
        Assert.False(wasMessageReceived);
    }

    [Fact]
    public void SelectedDrawingColor_ValueChanged_SendsDrawingColorChangedMessage()
    {
        // Arrange
        Color expected = GlobalDrawingValues.DrawingColors.Last();
        Color? actual = null;
        TestMessenger.Register<DrawingColorChangedMessage>(this, (_, msg) => actual = msg.Value);

        // Act
        _sut.SelectedDrawingColor = GlobalDrawingValues.DrawingColors.Last();

        // Assert
        Assert.Equal(expected, actual);
    }

    [Fact]
    public void SelectedDrawingColor_SetWithExistingValue_DoesNothing()
    {
        // Arrange
        var wasMessageReceived = false;
        TestMessenger.Register<DrawingColorChangedMessage>(this, (_, _) => wasMessageReceived = true);
        
        // Act
        _sut.SelectedDrawingColor = _sut.SelectedDrawingColor;

        // Assert
        Assert.False(wasMessageReceived);
    }

    [Fact]
    public void SelectedDrawingTool_ValueChanged_SendsDrawingToolChangedMessage()
    {
        // Arrange
        IDrawingTool expected = new RectangleTool();
        IDrawingTool? actual = null;
        TestMessenger.Register<DrawingToolChangedMessage>(this, (_, msg) => actual = msg.Value);

        // Act
        _sut.SelectedDrawingTool = new DrawingToolInfo(expected, "iconUri", "TestTool");

        // Assert
        Assert.Equal(expected, actual);
    }

    [Fact]
    public void SelectedDrawingTool_SetWithExistingValue_DoesNothing()
    {
        // Arrange
        var wasMessageReceived = false;
        TestMessenger.Register<DrawingToolChangedMessage>(this, (_, _) => wasMessageReceived = true);

        // Act
        _sut.SelectedDrawingTool = _sut.SelectedDrawingTool;

        // Assert
        Assert.False(wasMessageReceived);
    }

    [Fact]
    public void SelectedDrawingSize_ValueChanged_SendsDrawingSizeChangedMessage()
    {
        // Arrange
        double expected = 16;
        double actual = 0;
        TestMessenger.Register<DrawingSizeChangedMessage>(this, (_, msg) => actual = msg.Value);

        // Act
        _sut.SelectedDrawingSize = expected;

        // Assert
        Assert.Equal(expected, actual);
    }

    [Fact]
    public void SelectedDrawingSize_SetWithExistingValue_DoesNothing()
    {
        // Arrange
        var wasMessageReceived = false;
        TestMessenger.Register<DrawingSizeChangedMessage>(this, (_, _) => wasMessageReceived = true);

        // Act
        _sut.SelectedDrawingSize = _sut.SelectedDrawingSize;

        // Assert
        Assert.False(wasMessageReceived);
    }

    [Fact]
    public void SelectedDrawingSize_SetWithValueBelowMinimumDrawingSize_SetsValueToMinimumDrawingSize()
    {
        // Arrange
        double expected = GlobalDrawingValues.MinimumDrawingSize;

        // Act
        _sut.SelectedDrawingSize = GlobalDrawingValues.MinimumDrawingSize - 1;
        
        // Assert
        Assert.Equal(expected, _sut.SelectedDrawingSize);
    }

    [Fact]
    public void SelectedDrawingSize_SetWithValueAboveMaximumDrawingSize_SetsValueToMaximumDrawingSize()
    {
        // Arrange
        double expected = GlobalDrawingValues.MaximumDrawingSize;

        // Act
        _sut.SelectedDrawingSize = GlobalDrawingValues.MaximumDrawingSize + 1;

        // Assert
        Assert.Equal(expected, _sut.SelectedDrawingSize);
    }

    [Fact]
    public void Receive_DrawingWindowSetVisibilityMessage_SetsIsVisible()
    {
        // Arrange
        bool expected = !_sut.IsVisible;

        // Act
        TestMessenger.Send(new DrawingWindowSetVisibilityMessage(expected));

        // Act
        Assert.Equal(expected, _sut.IsVisible);
    }

    [Fact]
    public void Receive_DrawingWindowDragEventMessage_BeginDrag_SetsIsInputTransparentToTrue()
    {
        // Arrange
        _sut.IsInputTransparent = false;

        // Act
        TestMessenger.Send(new DrawingWindowDragEventMessage(DragAction.BeginDrag, new PointF()));

        // Assert
        Assert.True(_sut.IsInputTransparent);
    }

    [Fact]
    public void Receive_DrawingWindowDragEventMessage_EndDrag_SetsIsInputTransparentToFalse()
    {
        // Arrange
        _sut.IsInputTransparent = true;

        // Act
        TestMessenger.Send(new DrawingWindowDragEventMessage(DragAction.EndDrag, new PointF()));

        // Assert
        Assert.False(_sut.IsInputTransparent);
    }

    [Fact]
    public void Receive_DrawingWindowDragEventMessage_AllValues_SetsWindowMargin()
    {
        // Fact instead of Theory because BeginDrag must be called before ContinueDrag.
        var actions = new[]
        {
            DragAction.BeginDrag,
            DragAction.ContinueDrag,
            DragAction.EndDrag
        };

        foreach (DragAction action in actions)
        {
            // Arrange
            Thickness defaultValue = new();
            _sut.WindowMargin = defaultValue;
            PointF randomPoint = new (Random.Shared.Next(), Random.Shared.Next());

            // Act
            TestMessenger.Send(new DrawingWindowDragEventMessage(action, randomPoint));

            // Assert
            Assert.NotEqual(defaultValue, _sut.WindowMargin);
        }
    }

    [Fact]
    public void Receive_DrawingWindowDragEventMessage_ContinueDragBeforeBeginDrag_ThrowsInvalidOperationException()
    {
        Assert.Throws<InvalidOperationException>(() =>
            TestMessenger.Send(new DrawingWindowDragEventMessage(DragAction.ContinueDrag, new PointF())));
    }
}