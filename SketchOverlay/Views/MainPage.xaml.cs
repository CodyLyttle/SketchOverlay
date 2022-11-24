using CommunityToolkit.Mvvm.Messaging;
using SketchOverlay.Drawing.Canvas;
using SketchOverlay.Input;
using SketchOverlay.Messages;
using SketchOverlay.Messages.Actions;
using SketchOverlay.Models;
using SketchOverlay.Native.Input.Mouse;
using SketchOverlay.ViewModels;

namespace SketchOverlay.Views;

// TODO: Replace InputManager with mouse button WinUI handlers.
public partial class MainPage : ContentPage
{
    private readonly IInputManger _inputManager = new InputManager();
    private readonly IMessenger _messenger;

    // TODO: Inject DrawingCanvas into MainPage & OverlayWindowViewModel.
    public MainPage(OverlayWindowViewModel viewModel, IMessenger messenger, IDrawingCanvas canvas)
    {
        InitializeComponent();
        BindingContext = viewModel;
        _messenger = messenger;
        graphicsView.Drawable = canvas;

        graphicsView.StartInteraction += GraphicsViewOnStartInteraction;
        graphicsView.DragInteraction += GraphicsViewOnDragInteraction;
        graphicsView.EndInteraction += GraphicsViewOnEndInteraction;
        canvas.RequestRedraw += (_, _) => graphicsView.Invalidate();
        _inputManager.Events.MouseDown += OnMouseDown;
    }

    private void GraphicsViewOnStartInteraction(object? sender, TouchEventArgs e)
    {
        _messenger.Send(new OverlayWindowDrawActionMessage(
            new DrawActionInfo(
                DrawAction.BeginDraw,
                (MouseButton)GetLastMouseButton()!,
                e.Touches[0])));
    }

    private void GraphicsViewOnDragInteraction(object? sender, TouchEventArgs e)
    {
        _messenger.Send(new OverlayWindowDrawActionMessage(
            new DrawActionInfo(
                DrawAction.ContinueDraw, 
                (MouseButton)GetLastMouseButton()!, 
                e.Touches[0])));
    }

    private void GraphicsViewOnEndInteraction(object? sender, TouchEventArgs e)
    {
        _messenger.Send(new OverlayWindowDrawActionMessage(
            new DrawActionInfo(
                DrawAction.EndDraw,
                (MouseButton)GetLastMouseButton()!,
                e.Touches[0])));
    }
    
    private void OnMouseDown(object? sender, MouseButtonEventArgs args)
    {
        if (args.Button is MouseButton.Right)
            _messenger.Send(new OverlayWindowCancelDrawingMessage());
    }
    
    private MouseButton? GetLastMouseButton()
    {
        return _inputManager.State.LastMouseButtonEvent?.Button;
    }
}