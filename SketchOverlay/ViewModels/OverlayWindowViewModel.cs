using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using SketchOverlay.Drawing.Canvas;
using SketchOverlay.Messages;
using SketchOverlay.Messages.Actions;
using SketchOverlay.Native.Input.Mouse;

namespace SketchOverlay.ViewModels;

public partial class OverlayWindowViewModel : ObservableObject, 
    IRecipient<RequestCanvasActionMessage>,
    IRecipient<DrawingColorChangedMessage>,
    IRecipient<DrawingToolChangedMessage>,
    IRecipient<DrawingSizeChangedMessage>,
    IRecipient<OverlayWindowDrawActionMessage>,
    IRecipient<DrawingWindowVisibilityChangedMessage>,
    IRecipient<DrawingWindowIsDragInProgressChangedMessage>
{
    private readonly IMessenger _messenger;
    private bool _isToolWindowDragInProgress;
    private bool _isToolWindowVisible;

    public OverlayWindowViewModel(IDrawingCanvas canvas, IMessenger messenger)
    {
        _messenger = messenger;
        Canvas = canvas;
        messenger.Register<RequestCanvasActionMessage>(this);
        messenger.Register<DrawingColorChangedMessage>(this);
        messenger.Register<DrawingToolChangedMessage>(this);
        messenger.Register<DrawingSizeChangedMessage>(this);
        messenger.Register<OverlayWindowDrawActionMessage>(this);
        messenger.Register<DrawingWindowVisibilityChangedMessage>(this);
        messenger.Register<DrawingWindowIsDragInProgressChangedMessage>(this);
    }

    public IDrawingCanvas Canvas { get; }

    public void Receive(RequestCanvasActionMessage message)
    {
        switch (message.Value)
        {
            case CanvasAction.Undo:
                Canvas.Undo();
                break;
            case CanvasAction.Redo:
                Canvas.Redo();
                break;
            case CanvasAction.Clear:
                Canvas.Clear();
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(message));
        }
    }

    public void Receive(DrawingColorChangedMessage message)
    {
        Canvas.StrokeColor = message.Value;
    }

    public void Receive(DrawingToolChangedMessage message)
    {
        Canvas.DrawingTool = message.Value;
    }

    public void Receive(DrawingSizeChangedMessage message)
    {
        Canvas.StrokeSize = message.Value;
    }

    public void Receive(DrawingWindowVisibilityChangedMessage message)
    {
        _isToolWindowVisible = message.Value;
    }

    public void Receive(DrawingWindowIsDragInProgressChangedMessage message)
    {
        _isToolWindowDragInProgress = message.Value;
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

    private void DoDraw(PointF point)
    {
        Canvas.DoDrawingEvent(point);
    }

    private void EndDraw()
    {
        Canvas.FinalizeDrawingEvent();
    }

    private void BeginDraggingToolWindow(PointF cursorPos)
    {
        _messenger.Send(new DrawingWindowDragEventMessage(DragAction.BeginDrag, cursorPos));
        _messenger.Send(new DrawingWindowSetVisibilityMessage(true));
    }

    private void ContinueDraggingToolWindow(PointF cursorPos)
    {
        _messenger.Send(new DrawingWindowDragEventMessage(DragAction.ContinueDrag, cursorPos));
    }
    
    private void EndDraggingToolWindow(PointF cursorPos)
    {
        _messenger.Send(new DrawingWindowDragEventMessage(DragAction.EndDrag, cursorPos));
    }

    private void HideToolWindow()
    {
        _messenger.Send(new DrawingWindowSetVisibilityMessage(false));
    }
}