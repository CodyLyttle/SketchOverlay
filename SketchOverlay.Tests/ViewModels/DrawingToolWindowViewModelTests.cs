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
    private static readonly IMessenger TestMessenger = Globals.Messenger;
    private readonly DrawingToolWindowViewModel _sut;


    public DrawingToolWindowViewModelTests()
    {
        _sut = new DrawingToolWindowViewModel(TestMessenger);
    }

    [Fact]
    public void MessengerRegistered()
    {
        Assert.True(TestMessenger.IsRegistered<DrawingWindowSetVisibilityMessage>(_sut));
        Assert.True(TestMessenger.IsRegistered<DrawingWindowDragEventMessage>(_sut));
    }

    [Fact]
    public void UndoCommand_SendsRequestUndoMessage()
    {
        var received = CommonTests.AssertCommandSendsMessage<RequestCanvasActionMessage>(_sut.UndoCommand);
        Assert.Equal(CanvasAction.Undo, received.Value);
    }

    [Fact]
    public void RedoCommand_SendsRequestRedoMessage()
    {
        var received = CommonTests.AssertCommandSendsMessage<RequestCanvasActionMessage>(_sut.RedoCommand);
        Assert.Equal(CanvasAction.Redo, received.Value);
    }

    [Fact]
    public void ClearCommand_SendsRequestClearMessage()
    {
        var received = CommonTests.AssertCommandSendsMessage<RequestCanvasActionMessage>(_sut.ClearCommand);
        Assert.Equal(CanvasAction.Clear, received.Value);
    }

    [Fact]
    public void IsDragInProgress_ValueChanged_SendsDrawingWindowIsDragInProgressChangedMessage()
    {
        CommonTests.AssertPropertyChangedSendsMessageWithNewValue
            <DrawingWindowIsDragInProgressChangedMessage, bool>(_sut,
                nameof(DrawingToolWindowViewModel.IsDragInProgress),
                !_sut.IsDragInProgress);
    }

    [Fact]
    public void IsDragInProgress_SetWithExistingValue_DoesNotSendMessage()
    {
        CommonTests.AssertSetterWithSameValueDoesNotSendMessage
            <DrawingWindowIsDragInProgressChangedMessage, bool>(_sut,
                nameof(DrawingToolWindowViewModel.IsDragInProgress));
    }

    [Fact]
    public void IsVisible_ValueChanged_SendsDrawingWindowVisibilityChangedMessage()
    {
        CommonTests.AssertPropertyChangedSendsMessageWithNewValue
            <DrawingWindowVisibilityChangedMessage, bool>(_sut,
                nameof(DrawingToolWindowViewModel.IsVisible),
                !_sut.IsVisible);
    }

    [Fact]
    public void IsVisible_SetWithExistingValue_DoesNotSendMessage()
    {
        CommonTests.AssertSetterWithSameValueDoesNotSendMessage
            <DrawingWindowVisibilityChangedMessage, bool>(_sut,
                nameof(DrawingToolWindowViewModel.IsVisible));
    }

    [Fact]
    public void SelectedDrawingColor_ValueChanged_SendsDrawingColorChangedMessage()
    {
        CommonTests.AssertPropertyChangedSendsMessageWithNewValue
            <DrawingColorChangedMessage, Color>(_sut,
                nameof(DrawingToolWindowViewModel.SelectedDrawingColor),
                Colors.Bisque);
    }

    [Fact]
    public void SelectedDrawingColor_SetWithExistingValue_DoesNotSendMessage()
    {
        CommonTests.AssertSetterWithSameValueDoesNotSendMessage
            <DrawingColorChangedMessage, Color>(_sut,
                nameof(DrawingToolWindowViewModel.SelectedDrawingColor));
    }

    [Fact]
    public void SelectedDrawingTool_ValueChanged_SendsDrawingToolChangedMessage()
    {
        IDrawingTool expectedMessageValue = new RectangleTool();
        DrawingToolInfo newPropertyValue = new(expectedMessageValue, "iconUri", "TestTool");

        CommonTests.AssertPropertyChangedSendsMessageWithNewValue
            <DrawingToolChangedMessage, IDrawingTool, DrawingToolInfo>(_sut,
                nameof(DrawingToolWindowViewModel.SelectedDrawingTool),
                newPropertyValue,
                expectedMessageValue);
    }

    [Fact]
    public void SelectedDrawingTool_SetWithExistingValue_DoesNotSendMessage()
    {
        CommonTests.AssertSetterWithSameValueDoesNotSendMessage
            <DrawingToolChangedMessage, DrawingToolInfo>(_sut,
                nameof(DrawingToolWindowViewModel.SelectedDrawingTool));
    }

    [Fact]
    public void SelectedDrawingSize_ValueChanged_SendsDrawingSizeChangedMessage()
    {
        CommonTests.AssertPropertyChangedSendsMessageWithNewValue
            <DrawingSizeChangedMessage, float, double>(_sut,
                nameof(DrawingToolWindowViewModel.SelectedDrawingSize),
                _sut.SelectedDrawingSize + 1,
                (float)_sut.SelectedDrawingSize + 1);
    }

    [Fact]
    public void SelectedDrawingSize_SetWithExistingValue_DoesNotSendMessage()
    {
        CommonTests.AssertSetterWithSameValueDoesNotSendMessage
            <DrawingSizeChangedMessage, double>(_sut,
                nameof(DrawingToolWindowViewModel.SelectedDrawingSize));
    }

    [Fact]
    public void SelectedDrawingSize_SetWithValueBelowMinimumDrawingSize_SetsValueToMinimumDrawingSize()
    {
        // Arrange
        const double expected = GlobalDrawingValues.MinimumDrawingSize;

        // Act
        _sut.SelectedDrawingSize = GlobalDrawingValues.MinimumDrawingSize - 1;

        // Assert
        Assert.Equal(expected, _sut.SelectedDrawingSize);
    }

    [Fact]
    public void SelectedDrawingSize_SetWithValueAboveMaximumDrawingSize_SetsValueToMaximumDrawingSize()
    {
        // Arrange
        const double expected = GlobalDrawingValues.MaximumDrawingSize;

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
        _sut.Receive(new DrawingWindowSetVisibilityMessage(expected));

        // Act
        Assert.Equal(expected, _sut.IsVisible);
    }

    [Fact]
    public void Receive_BeginDrag_SetsIsDragInProgressToTrue()
    {
        // Arrange
        Assert.False(_sut.IsDragInProgress);

        // Act
        _sut.Receive(new DrawingWindowDragEventMessage(DragAction.BeginDrag, new PointF()));

        // Assert
        Assert.True(_sut.IsDragInProgress);
    }

    [Fact]
    public void Receive_EndDrag_SetsIsDragInProgressToToFalse()
    {
        // Arrange
        _sut.Receive(new DrawingWindowDragEventMessage(DragAction.BeginDrag, new PointF()));
        Assert.True(_sut.IsDragInProgress);

        // Act
        _sut.Receive(new DrawingWindowDragEventMessage(DragAction.EndDrag, new PointF()));

        // Assert
        Assert.False(_sut.IsDragInProgress);
    }

    [Fact]
    public void Receive_AllDragActions_SetsWindowMargin()
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
            PointF randomPoint = new(Random.Shared.Next(), Random.Shared.Next());

            // Act
            _sut.Receive(new DrawingWindowDragEventMessage(action, randomPoint));

            // Assert
            Assert.NotEqual(defaultValue, _sut.WindowMargin);
        }
    }

    [Fact]
    public void Receive_BeginDragWhileDragging_ThrowsInvalidOperationException()
    {
        // Arrange
        _sut.Receive(new DrawingWindowDragEventMessage(DragAction.BeginDrag, new PointF()));
        Assert.True(_sut.IsDragInProgress);

        // Assert
        Assert.Throws<InvalidOperationException>(() =>
            _sut.Receive(new DrawingWindowDragEventMessage(DragAction.BeginDrag, new PointF())));
    }

    [Fact]
    public void Receive_ContinueDragWhileNotDragging_ThrowsInvalidOperationException()
    {
        // Arrange
        Assert.False(_sut.IsDragInProgress);

        // Assert
        Assert.Throws<InvalidOperationException>(() =>
            _sut.Receive(new DrawingWindowDragEventMessage(DragAction.ContinueDrag, new PointF())));
    }

    [Fact]
    public void Receive_EndDragWhileNotDragging_ThrowsInvalidOperationException()
    {
        // Arrange
        Assert.False(_sut.IsDragInProgress);

        // Assert
        Assert.Throws<InvalidOperationException>(() =>
            _sut.Receive(new DrawingWindowDragEventMessage(DragAction.ContinueDrag, new PointF())));
    }
}