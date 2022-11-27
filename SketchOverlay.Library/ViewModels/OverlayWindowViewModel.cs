using System.Drawing;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using SketchOverlay.Library.Drawing;
using SketchOverlay.Library.Messages;
using SketchOverlay.Library.Models;

namespace SketchOverlay.Library.ViewModels;

public partial class OverlayWindowViewModel<TDrawing, TImageSource> : ObservableObject,
    IRecipient<DrawingWindowPropertyChangedMessage>,
    IRecipient<OverlayWindowCanvasActionMessage>
{
    private readonly IDrawingCanvas<TDrawing> _canvas;
    private readonly IMessenger _messenger;
    private bool _isToolWindowDragInProgress;
    private bool _isToolWindowVisible;

    public OverlayWindowViewModel(IDrawingCanvas<TDrawing> canvas, IMessenger messenger)
    {
        _canvas = canvas;
        _canvas.CanClearChanged += (_, value) => SetDrawingWindowCanClear(value);
        _canvas.CanRedoChanged += (_, value) => SetDrawingWindowCanRedo(value);
        _canvas.CanUndoChanged += (_, value) => SetDrawingWindowCanUndo(value);

        _messenger = messenger;
        messenger.Register<DrawingWindowPropertyChangedMessage>(this);
        messenger.Register<OverlayWindowCanvasActionMessage>(this);
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
            case nameof(DrawingToolWindowViewModel<TDrawing, TImageSource>.SelectedDrawingTool):
                _canvas.DrawingTool = ((DrawingToolInfo<TDrawing, TImageSource>)message.Value).Tool;
                break;
            case nameof(DrawingToolWindowViewModel<TDrawing, TImageSource>.IsVisible):
                _isToolWindowVisible = (bool)message.Value;
                break;
            case nameof(DrawingToolWindowViewModel<TDrawing, TImageSource>.IsDragInProgress):
                _isToolWindowDragInProgress = (bool)message.Value;
                break;
        }
    }

    [RelayCommand]
    private void MouseDown(MouseActionInfo info)
    {
        if (info.Button is MouseButton.Left)
        {
            // Allow drawing to pass behind tool window.
            SetDrawingWindowInputTransparency(true);
            _canvas.DoDrawingEvent(info.CursorPosition);
        }
        else if (info.Button is MouseButton.Middle)
        {
            if (_isToolWindowVisible)
            {
                SetDrawingWindowVisibility(false);
            }
            else
            {
                SendOverlayWindowDragAction(DragAction.BeginDrag, info.CursorPosition);
                SetDrawingWindowVisibility(true);
            }
        }
    }

    [RelayCommand]
    private void MouseDrag(MouseActionInfo info)
    {
        if (info.Button is MouseButton.Left)
        {
            _canvas.DoDrawingEvent(info.CursorPosition);
        }
        else if (info.Button is MouseButton.Middle && _isToolWindowDragInProgress)
        {
            SendOverlayWindowDragAction(DragAction.ContinueDrag, info.CursorPosition);
        }
    }

    [RelayCommand]
    private void MouseUp(MouseActionInfo info)
    {
        if (info.Button is MouseButton.Left)
        {
            SetDrawingWindowInputTransparency(false);
            _canvas.FinalizeDrawingEvent();
        }
        else if (info.Button is MouseButton.Middle && _isToolWindowDragInProgress)
        {
            SendOverlayWindowDragAction(DragAction.EndDrag, info.CursorPosition);
        }
    }

    private void SendOverlayWindowDragAction(DragAction action, PointF cursorPos)
    {
        _messenger.Send(new DrawingWindowDragEventMessage(action, cursorPos));
    }

    private void SetDrawingWindowCanClear(bool canClear)
    {
        _messenger.Send(new DrawingWindowSetPropertyMessage(
            nameof(DrawingToolWindowViewModel<TDrawing, TImageSource>.CanClear),
            canClear));
    }

    private void SetDrawingWindowCanRedo(bool canRedo)
    {
        _messenger.Send(new DrawingWindowSetPropertyMessage(
            nameof(DrawingToolWindowViewModel<TDrawing, TImageSource>.CanRedo),
            canRedo));
    }

    private void SetDrawingWindowCanUndo(bool canUndo)
    {
        _messenger.Send(new DrawingWindowSetPropertyMessage(
            nameof(DrawingToolWindowViewModel<TDrawing, TImageSource>.CanUndo),
            canUndo));
    }

    private void SetDrawingWindowVisibility(bool isVisible)
    {

        _messenger.Send(new DrawingWindowSetPropertyMessage(
            nameof(DrawingToolWindowViewModel<TDrawing, TImageSource>.IsVisible),
            isVisible));
    }

    private void SetDrawingWindowInputTransparency(bool isInputTransparent)
    {
        _messenger.Send(new DrawingWindowSetPropertyMessage(
            nameof(DrawingToolWindowViewModel<TDrawing, TImageSource>.IsInputTransparent),
            isInputTransparent));
    }
}