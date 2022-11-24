using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using SketchOverlay.Drawing.Canvas;
using SketchOverlay.Messages;
using SketchOverlay.Messages.Actions;
using SketchOverlay.Models;
using SketchOverlay.Native.Input.Mouse;

namespace SketchOverlay.ViewModels;

public partial class OverlayWindowViewModel : ObservableObject,
    IRecipient<DrawingWindowPropertyChangedMessage>,
    IRecipient<OverlayWindowCanvasActionMessage>,
    IRecipient<OverlayWindowDrawActionMessage>,
    IRecipient<OverlayWindowCancelDrawingMessage>
{
    private readonly IDrawingCanvas _canvas;
    private readonly IMessenger _messenger;
    private bool _isToolWindowDragInProgress;
    private bool _isToolWindowVisible;

    public OverlayWindowViewModel(IDrawingCanvas canvas, IMessenger messenger)
    {
        _canvas = canvas;
        _canvas.CanClearChanged += (_, value) => SetDrawingWindowCanClear(value);
        _canvas.CanRedoChanged += (_, value) => SetDrawingWindowCanRedo(value);
        _canvas.CanUndoChanged += (_, value) => SetDrawingWindowCanUndo(value);
        
        _messenger = messenger;
        messenger.Register<DrawingWindowPropertyChangedMessage>(this);
        messenger.Register<OverlayWindowCanvasActionMessage>(this);
        messenger.Register<OverlayWindowDrawActionMessage>(this);
        messenger.Register<OverlayWindowCancelDrawingMessage>(this);
    }

    public void Receive(OverlayWindowCanvasActionMessage message)
    {
        switch (message.Value)
        {
            case CanvasAction.Undo:
                _canvas.Undo();
                break;
            case CanvasAction.Redo:
                _canvas.Redo();
                break;
            case CanvasAction.Clear:
                _canvas.Clear();
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(message));
        }
    }

    public void Receive(DrawingWindowPropertyChangedMessage message)
    {
        switch (message.PropertyName)
        {
            case nameof(DrawingToolWindowViewModel.SelectedDrawingColor):
                _canvas.StrokeColor = (Color)message.Value;
                break;
            case nameof(DrawingToolWindowViewModel.SelectedDrawingSize):
                _canvas.StrokeSize = Convert.ToSingle(message.Value);
                break;
            case nameof(DrawingToolWindowViewModel.SelectedDrawingTool):
                _canvas.DrawingTool = ((DrawingToolInfo)message.Value).Tool;
                break;
            case nameof(DrawingToolWindowViewModel.IsVisible):
                _isToolWindowVisible = (bool)message.Value;
                break;
            case nameof(DrawingToolWindowViewModel.IsDragInProgress):
                _isToolWindowDragInProgress = (bool)message.Value;
                break;
        }
    }

    public void Receive(OverlayWindowDrawActionMessage message)
    {
        DrawAction action = message.Value.Action;
        MouseButton button = message.Value.Button;
        PointF cursorPos = message.Value.CursorPosition;

        if (action is DrawAction.BeginDraw)
        {
            if (button is MouseButton.Left)
            {
                // TODO: Allow drawing to continue beneath the tool window by disabling it's hit testing.
                DoDraw(cursorPos);
            }
            else if (button is MouseButton.Middle)
            {
                if (_isToolWindowVisible)
                    HideToolWindow();
                else
                    BeginDraggingToolWindow(cursorPos);

            }
        }
        else if (action is DrawAction.ContinueDraw)
        {
            if (button is MouseButton.Left)
            {
                DoDraw(cursorPos);
            }
            else if (button is MouseButton.Middle && _isToolWindowDragInProgress)
            {
                ContinueDraggingToolWindow(cursorPos);
            }
        }
        else if (action is DrawAction.EndDraw)
        {
            if (button is MouseButton.Left)
            {
                // TODO: Restore hit testing on tool window.
                EndDraw();
            }
            else if (button is MouseButton.Middle && _isToolWindowDragInProgress)
            {
                EndDraggingToolWindow(cursorPos);
            }
        }
    }

    public void Receive(OverlayWindowCancelDrawingMessage message)
    {
        _canvas.CancelDrawingEvent();
    }

    private void DoDraw(PointF point)
    {
        SetDrawingWindowInputTransparency(true);
        _canvas.DoDrawingEvent(point);
    }

    private void EndDraw()
    {
        SetDrawingWindowInputTransparency(false);
        _canvas.FinalizeDrawingEvent();
    }

    private void BeginDraggingToolWindow(PointF cursorPos)
    {
        SendOverlayWindowDragAction(DragAction.BeginDrag, cursorPos);
        SetDrawingWindowVisibility(true);
    }

    private void ContinueDraggingToolWindow(PointF cursorPos)
    {
        SendOverlayWindowDragAction(DragAction.ContinueDrag, cursorPos);
    }

    private void EndDraggingToolWindow(PointF cursorPos)
    {
        SendOverlayWindowDragAction(DragAction.EndDrag, cursorPos);
    }

    private void HideToolWindow()
    {
        SetDrawingWindowVisibility(false);
    }

    private void SetDrawingWindowCanClear(bool canClear)
    {
        _messenger.Send(new DrawingWindowSetPropertyMessage(
            nameof(DrawingToolWindowViewModel.CanClear), 
            canClear));
    }

    private void SetDrawingWindowCanRedo(bool canRedo)
    {
        _messenger.Send(new DrawingWindowSetPropertyMessage(
            nameof(DrawingToolWindowViewModel.CanRedo),
            canRedo));
    }

    private void SetDrawingWindowCanUndo(bool canUndo)
    {
        _messenger.Send(new DrawingWindowSetPropertyMessage(
            nameof(DrawingToolWindowViewModel.CanUndo),
            canUndo));
    }

    private void SetDrawingWindowVisibility(bool isVisible)
    {

        _messenger.Send(new DrawingWindowSetPropertyMessage(
            nameof(DrawingToolWindowViewModel.IsVisible),
            isVisible));
    }

    private void SetDrawingWindowInputTransparency(bool isInputTransparent)
    {
        _messenger.Send(new DrawingWindowSetPropertyMessage(
            nameof(DrawingToolWindowViewModel.IsInputTransparent),
            isInputTransparent));
    }

    private void SendOverlayWindowDragAction(DragAction action, PointF cursorPos)
    {
        _messenger.Send(new DrawingWindowDragEventMessage(action, cursorPos));
    }
}