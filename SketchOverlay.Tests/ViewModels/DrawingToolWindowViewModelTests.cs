using CommunityToolkit.Mvvm.Messaging;
using SketchOverlay.Drawing;
using SketchOverlay.Drawing.Tools;
using SketchOverlay.Messages;
using SketchOverlay.Messages.Actions;
using SketchOverlay.Models;
using SUT = SketchOverlay.ViewModels.DrawingToolWindowViewModel;

namespace SketchOverlay.Tests.ViewModels;

public class DrawingToolWindowViewModelTests
{
    private static readonly IMessenger TestMessenger = Globals.Messenger;
    private readonly SUT _sut;

    public DrawingToolWindowViewModelTests()
    {
        _sut = new SUT(TestMessenger);
    }

    private static DrawingWindowDragEventMessage CreateDragMessage(DragAction action)
    {
        return new DrawingWindowDragEventMessage(action, new PointF(
            Random.Shared.Next(),
            Random.Shared.Next()));
    }

    private static DrawingWindowSetPropertyMessage CreateSetMessage(string propertyName, object value)
    {
        return new DrawingWindowSetPropertyMessage(propertyName, value);
    }

    [Fact]
    public void MessengerRegistered()
    {
        Assert.True(TestMessenger.IsRegistered<DrawingWindowSetPropertyMessage>(_sut));
        Assert.True(TestMessenger.IsRegistered<DrawingWindowDragEventMessage>(_sut));
    }

    [Fact]
    public void UndoCommand_SendsRequestUndoMessage()
    {
        var received = CommonTests.AssertCommandSendsMessage
            <OverlayWindowCanvasActionMessage>(_sut.UndoCommand);

        Assert.Equal(CanvasAction.Undo, received.Value);
    }

    [Fact]
    public void RedoCommand_SendsRequestRedoMessage()
    {
        var received = CommonTests.AssertCommandSendsMessage
            <OverlayWindowCanvasActionMessage>(_sut.RedoCommand);

        Assert.Equal(CanvasAction.Redo, received.Value);
    }

    [Fact]
    public void ClearCommand_SendsRequestClearMessage()
    {
        var received = CommonTests.AssertCommandSendsMessage
            <OverlayWindowCanvasActionMessage>(_sut.ClearCommand);

        Assert.Equal(CanvasAction.Clear, received.Value);
    }

    [Fact]
    public void IsDragInProgress_ValueChanged_SendsPropertyChangedMessage()
    {
        CommonTests.AssertPropertyChangedSendsSimplePropertyChangedMessage
            <DrawingWindowPropertyChangedMessage, bool>(_sut,
                nameof(SUT.IsDragInProgress),
                !_sut.IsDragInProgress);
    }

    [Fact]
    public void IsVisible_ValueChanged_SendsPropertyChangedMessage()
    {
        CommonTests.AssertPropertyChangedSendsSimplePropertyChangedMessage
            <DrawingWindowPropertyChangedMessage, bool>(_sut,
                nameof(SUT.IsVisible),
                !_sut.IsVisible);
    }

    [Fact]
    public void SelectedDrawingColor_ValueChanged_SendsPropertyChangedMessage()
    {
        CommonTests.AssertPropertyChangedSendsSimplePropertyChangedMessage
            <DrawingWindowPropertyChangedMessage, Color>(_sut,
                nameof(SUT.SelectedDrawingColor),
                Colors.Bisque);
    }

    [Fact]
    public void SelectedDrawingTool_ValueChanged_SendsPropertyChangedMessage()
    {
        CommonTests.AssertPropertyChangedSendsSimplePropertyChangedMessage
            <DrawingWindowPropertyChangedMessage, DrawingToolInfo>(_sut,
                nameof(SUT.SelectedDrawingTool),
                new DrawingToolInfo(new RectangleTool(), "iconUri", "TestTool"));
    }

    [Fact]
    public void SelectedDrawingSize_ValueChanged_SendsPropertyChangedMessage()
    {
        CommonTests.AssertPropertyChangedSendsSimplePropertyChangedMessage
            <DrawingWindowPropertyChangedMessage, double>(_sut,
                nameof(SUT.SelectedDrawingSize),
                _sut.SelectedDrawingSize + 1);
    }

    [Fact]
    public void IsDragInProgress_SetWithExistingValue_DoesNotSendMessage()
    {
        CommonTests.AssertSetterWithSameValueDoesNotSendMessage
            <DrawingWindowPropertyChangedMessage, bool>(_sut,
                nameof(SUT.IsDragInProgress));
    }

    [Fact]
    public void IsVisible_SetWithExistingValue_DoesNotSendMessage()
    {
        CommonTests.AssertSetterWithSameValueDoesNotSendMessage
            <DrawingWindowPropertyChangedMessage, bool>(_sut,
                nameof(SUT.IsVisible));
    }

    [Fact]
    public void SelectedDrawingColor_SetWithExistingValue_DoesNotSendMessage()
    {
        CommonTests.AssertSetterWithSameValueDoesNotSendMessage
            <DrawingWindowPropertyChangedMessage, Color>(_sut,
                nameof(SUT.SelectedDrawingColor));
    }

    [Fact]
    public void SelectedDrawingTool_SetWithExistingValue_DoesNotSendMessage()
    {
        CommonTests.AssertSetterWithSameValueDoesNotSendMessage
            <DrawingWindowPropertyChangedMessage, DrawingToolInfo>(_sut,
                nameof(SUT.SelectedDrawingTool));
    }

    [Fact]
    public void SelectedDrawingSize_SetWithExistingValue_DoesNotSendMessage()
    {
        CommonTests.AssertSetterWithSameValueDoesNotSendMessage
            <DrawingWindowPropertyChangedMessage, double>(_sut,
                nameof(SUT.SelectedDrawingSize));
    }

    [Fact]
    public void SelectedDrawingSize_SetWithValueBelowMinimumDrawingSize_SetsValueToMinimumDrawingSize()
    {
        CommonTests.AssertSetterModifiesInputValue(_sut, 
            nameof(SUT.SelectedDrawingSize),
            GlobalDrawingValues.MinimumDrawingSize - 1,
            GlobalDrawingValues.MinimumDrawingSize);
    }

    [Fact]
    public void SelectedDrawingSize_SetWithValueAboveMaximumDrawingSize_SetsValueToMaximumDrawingSize()
    {
        CommonTests.AssertSetterModifiesInputValue(_sut,
            nameof(SUT.SelectedDrawingSize),
            GlobalDrawingValues.MaximumDrawingSize + 1,
            GlobalDrawingValues.MaximumDrawingSize);
    }

    [Fact]
    public void Receive_SetPropertyMessage_SetsIsVisible()
    {
        CommonTests.AssertReceiveMessageUpdatesProperty(_sut, _sut.Receive,
            CreateSetMessage(nameof(SUT.IsVisible), !_sut.IsVisible));
    }

    [Fact]
    public void Receive_SetPropertyMessage_SetsIsInputTransparent()
    {
        CommonTests.AssertReceiveMessageUpdatesProperty(_sut, _sut.Receive,
            CreateSetMessage(nameof(SUT.IsInputTransparent), !_sut.IsInputTransparent));
    }

    [Fact]
    public void Receive_BeginDrag_ModifiesProperties()
    {
        // Arrange
        Assert.False(_sut.IsDragInProgress);
        Assert.False(_sut.IsInputTransparent);

        // Act
        _sut.Receive(CreateDragMessage(DragAction.BeginDrag));

        // Assert
        Assert.True(_sut.IsDragInProgress);
        Assert.True(_sut.IsInputTransparent);
    }

    [Fact]
    public void Receive_EndDrag_ModifiesProperties()
    {
        // Arrange
        _sut.Receive(CreateDragMessage(DragAction.BeginDrag));
        Assert.True(_sut.IsDragInProgress);
        Assert.True(_sut.IsInputTransparent);

        // Act
        _sut.Receive(CreateDragMessage(DragAction.EndDrag));

        // Assert
        Assert.False(_sut.IsDragInProgress);
        Assert.False(_sut.IsInputTransparent);
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

            // Act
            _sut.Receive(CreateDragMessage(action));

            // Assert
            Assert.NotEqual(defaultValue, _sut.WindowMargin);
        }
    }

    [Fact]
    public void Receive_BeginDragWhileDragging_ThrowsInvalidOperationException()
    {
        // Arrange
        _sut.Receive(CreateDragMessage(DragAction.BeginDrag));
        Assert.True(_sut.IsDragInProgress);

        // Assert
        Assert.Throws<InvalidOperationException>(() =>
            _sut.Receive(CreateDragMessage(DragAction.BeginDrag)));
    }

    [Fact]
    public void Receive_ContinueDragWhileNotDragging_ThrowsInvalidOperationException()
    {
        // Arrange
        Assert.False(_sut.IsDragInProgress);

        // Assert
        Assert.Throws<InvalidOperationException>(() =>
            _sut.Receive(CreateDragMessage(DragAction.ContinueDrag)));
    }

    [Fact]
    public void Receive_EndDragWhileNotDragging_ThrowsInvalidOperationException()
    {
        // Arrange
        Assert.False(_sut.IsDragInProgress);

        // Assert
        Assert.Throws<InvalidOperationException>(() =>
            _sut.Receive(CreateDragMessage(DragAction.EndDrag)));
    }
}