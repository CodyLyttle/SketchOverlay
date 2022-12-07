using System.Drawing;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using SketchOverlay.Library.Drawing.Canvas;
using SketchOverlay.Library.Messages;
using SketchOverlay.Library.Models;

namespace SketchOverlay.Library.ViewModels;

public partial class OverlayWindowViewModel<TDrawing, TOutput, TImageSource, TColor> : ObservableObject,
    IRecipient<ToolsWindowPropertyChangedMessage>
{
    private readonly ICanvasDrawingManager<TOutput> _canvasManager;
    private readonly IMessenger _messenger;
    private bool _isToolsWindowDragInProgress;
    private bool _isToolsWindowVisible;

    public OverlayWindowViewModel(ICanvasDrawingManager<TOutput> canvasManager, IMessenger messenger)
    {
        _canvasManager = canvasManager;
        _messenger = messenger;
        messenger.Register(this);
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