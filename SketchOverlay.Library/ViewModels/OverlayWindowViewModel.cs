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
    IRecipient<ToolsWindowPropertyChangedMessage>,
    IRecipient<OverlayWindowCanvasActionMessage>
{
    private readonly ICanvasManager<TOutput> _canvasManager;
    private readonly IMessenger _messenger;
    private bool _isToolsWindowDragInProgress;
    private bool _isToolsWindowVisible;

    public OverlayWindowViewModel(ICanvasManager<TOutput> canvasManager, IMessenger messenger)
    {
        _canvasManager = canvasManager;
        _canvasManager.CanClearChanged += (_, value) => SetDrawingWindowCanClear(value);
        _canvasManager.CanRedoChanged += (_, value) => SetDrawingWindowCanRedo(value);
        _canvasManager.CanUndoChanged += (_, value) => SetDrawingWindowCanUndo(value);

        _messenger = messenger;
        messenger.Register<ToolsWindowPropertyChangedMessage>(this);
        messenger.Register<OverlayWindowCanvasActionMessage>(this);
    }

    [ObservableProperty] 
    private bool _isCanvasVisible = true;

    [RelayCommand]
    private void ToggleCanvasVisibility()
    {
        IsCanvasVisible = !IsCanvasVisible;
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
            if (_isToolsWindowVisible)
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
        else if (info.Button is MouseButton.Middle && _isToolsWindowDragInProgress)
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
        else if (info.Button is MouseButton.Middle && _isToolsWindowDragInProgress)
        {
            SendOverlayWindowDragAction(DragAction.EndDrag, info.CursorPosition);
        }
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

    public void Receive(ToolsWindowPropertyChangedMessage message)
    {
        switch (message.PropertyName)
        {
            case nameof(ToolsWindowViewModel<TDrawing, TImageSource, TColor>.IsVisible):
                _isToolsWindowVisible = (bool)message.Value;
                break;
            case nameof(ToolsWindowViewModel<TDrawing, TImageSource, TColor>.IsDragInProgress):
                _isToolsWindowDragInProgress = (bool)message.Value;
                break;
        }
    }

    private void SendOverlayWindowDragAction(DragAction action, PointF cursorPos)
    {
        _messenger.Send(new ToolsWindowDragEventMessage(action, cursorPos));
    }

    private void SetDrawingWindowCanClear(bool canClear)
    {
        _messenger.Send(new ToolsWindowSetPropertyMessage(
            nameof(ToolsWindowViewModel<TDrawing, TImageSource, TColor>.CanClear),
            canClear));
    }

    private void SetDrawingWindowCanRedo(bool canRedo)
    {
        _messenger.Send(new ToolsWindowSetPropertyMessage(
            nameof(ToolsWindowViewModel<TDrawing, TImageSource, TColor>.CanRedo),
            canRedo));
    }

    private void SetDrawingWindowCanUndo(bool canUndo)
    {
        _messenger.Send(new ToolsWindowSetPropertyMessage(
            nameof(ToolsWindowViewModel<TDrawing, TImageSource, TColor>.CanUndo),
            canUndo));
    }

    private void SetDrawingWindowVisibility(bool isVisible)
    {

        _messenger.Send(new ToolsWindowSetPropertyMessage(
            nameof(ToolsWindowViewModel<TDrawing, TImageSource, TColor>.IsVisible),
            isVisible));
    }

    private void SetDrawingWindowInputTransparency(bool isInputTransparent)
    {
        _messenger.Send(new ToolsWindowSetPropertyMessage(
            nameof(ToolsWindowViewModel<TDrawing, TImageSource, TColor>.IsInputTransparent),
            isInputTransparent));
    }
}