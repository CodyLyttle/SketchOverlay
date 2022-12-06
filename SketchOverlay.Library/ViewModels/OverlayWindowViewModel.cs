using System.Drawing;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using SketchOverlay.Library.Drawing.Canvas;
using SketchOverlay.Library.Messages;
using SketchOverlay.Library.Models;

namespace SketchOverlay.Library.ViewModels;

// TODO: Tests.
public partial class OverlayWindowViewModel<TDrawing, TOutput, TImageSource, TColor> : ObservableObject,
    IRecipient<DrawingWindowPropertyChangedMessage>,
    IRecipient<OverlayWindowCanvasActionMessage>
{
    private readonly ICanvasManager<TOutput> _canvasManager;
    private readonly IMessenger _messenger;
    private bool _isToolWindowDragInProgress;
    private bool _isToolWindowVisible;

    public OverlayWindowViewModel(ICanvasManager<TOutput> canvasManager, IMessenger messenger)
    {
        _canvasManager = canvasManager;
        _canvasManager.CanClearChanged += (_, value) => SetDrawingWindowCanClear(value);
        _canvasManager.CanRedoChanged += (_, value) => SetDrawingWindowCanRedo(value);
        _canvasManager.CanUndoChanged += (_, value) => SetDrawingWindowCanUndo(value);

        _messenger = messenger;
        messenger.Register<DrawingWindowPropertyChangedMessage>(this);
        messenger.Register<OverlayWindowCanvasActionMessage>(this);
    }

    [ObservableProperty] 
    private bool _isCanvasVisible = true;

    [RelayCommand]
    private void ToggleCanvasVisibility()
    {
        IsCanvasVisible = !IsCanvasVisible;
    }

    public void Receive(OverlayWindowCanvasActionMessage message)
    {
        switch (message.Value)
        {
            case CanvasAction.Undo:
                _canvasManager.Undo();
                break;
            case CanvasAction.Redo:
                _canvasManager.Redo();
                break;
            case CanvasAction.Clear:
                _canvasManager.Clear();
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(message));
        }
    }

    public void Receive(DrawingWindowPropertyChangedMessage message)
    {
        switch (message.PropertyName)
        {
            case nameof(DrawingToolWindowViewModel<TDrawing, TImageSource, TColor>.IsVisible):
                _isToolWindowVisible = (bool)message.Value;
                break;
            case nameof(DrawingToolWindowViewModel<TDrawing, TImageSource, TColor>.IsDragInProgress):
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
            _canvasManager.DoDrawing(info.CursorPosition);
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
            _canvasManager.DoDrawing(info.CursorPosition);
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
            _canvasManager.FinishDrawing();
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
            nameof(DrawingToolWindowViewModel<TDrawing, TImageSource, TColor>.CanClear),
            canClear));
    }

    private void SetDrawingWindowCanRedo(bool canRedo)
    {
        _messenger.Send(new DrawingWindowSetPropertyMessage(
            nameof(DrawingToolWindowViewModel<TDrawing, TImageSource, TColor>.CanRedo),
            canRedo));
    }

    private void SetDrawingWindowCanUndo(bool canUndo)
    {
        _messenger.Send(new DrawingWindowSetPropertyMessage(
            nameof(DrawingToolWindowViewModel<TDrawing, TImageSource, TColor>.CanUndo),
            canUndo));
    }

    private void SetDrawingWindowVisibility(bool isVisible)
    {

        _messenger.Send(new DrawingWindowSetPropertyMessage(
            nameof(DrawingToolWindowViewModel<TDrawing, TImageSource, TColor>.IsVisible),
            isVisible));
    }

    private void SetDrawingWindowInputTransparency(bool isInputTransparent)
    {
        _messenger.Send(new DrawingWindowSetPropertyMessage(
            nameof(DrawingToolWindowViewModel<TDrawing, TImageSource, TColor>.IsInputTransparent),
            isInputTransparent));
    }
}