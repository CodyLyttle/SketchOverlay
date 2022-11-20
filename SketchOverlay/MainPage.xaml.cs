using System.Diagnostics;
using SketchOverlay.DrawingTools;
using SketchOverlay.Input;
using SketchOverlay.Native;
using SketchOverlay.Native.Input.Mouse;

namespace SketchOverlay;

// TODO: Use InputManager.Events to handle the tool window dragging logic. Dragging stops abruptly due to the cursor leaving the bounds of the GraphicsView.
public partial class MainPage : ContentPage
{
    private const MouseButton ToggleDrawingWindowButton = MouseButton.Right;

    private readonly DrawingCanvas _rootDrawable = new(new BrushTool());
    private readonly IInputManger _inputManager;
    private IntPtr _windowHandle;

    // TODO: Inject IInputManager via constructor & MauiProgram.cs
    public MainPage()
    {
        InitializeComponent();

        canvas.Drawable = _rootDrawable;
        canvas.StartInteraction += CanvasOnStartInteraction;
        canvas.DragInteraction += CanvasOnDragInteraction;
        canvas.EndInteraction += CanvasOnEndInteraction;
        _rootDrawable.RequestRedraw += (_, _) => canvas.Invalidate();

        _inputManager = new InputManager();
        _inputManager.Events.MouseMove += (sender, args) =>
        {
            Dispatcher.Dispatch(() => debugLabel.Text = GetRelativePosition(args.Position).ToString());
        };
    }

    private void CanvasOnStartInteraction(object? sender, TouchEventArgs e)
    {
        if (_inputManager.State.IsButtonDown(MouseButton.Left))
        {
            // Allow drawing to continue when the cursor enters the drawing tool window.
            DrawingToolsWindow.InputTransparent = true;
        }

        else if (_inputManager.State.IsButtonDown(ToggleDrawingWindowButton))
        {
            if (DrawingToolsWindow.IsVisible)
            {
                DrawingToolsWindow.HideWindow();
            }
            else
            {
                DrawingToolsWindow.BeginDragWindow(e.Touches[0]);
                DrawingToolsWindow.ShowWindow();
            }
        }

        CanvasOnDragInteraction(sender, e);
    }

    private void CanvasOnDragInteraction(object? sender, TouchEventArgs e)
    {
        PointF cursorPos = e.Touches[0];

        if (_inputManager.State.IsButtonDown(MouseButton.Left))
        {
            _rootDrawable.DoDrawingEvent(cursorPos);
        }
        else if (_inputManager.State.IsButtonDown(MouseButton.Right) &&
                 DrawingToolsWindow.IsDragging)
        {
            DrawingToolsWindow.ContinueDragWindow(cursorPos);
        }
    }

    private void CanvasOnEndInteraction(object? sender, TouchEventArgs e)
    {
        MouseButtonEventArgs? lastMouseEvent = _inputManager.State.GetLastMouseButtonEvent;
        if (lastMouseEvent == null)
            return;

        if (lastMouseEvent.Button == MouseButton.Left)
        {
            _rootDrawable.EndDrawingEvent();
            DrawingToolsWindow.InputTransparent = false;
        }
        else if (lastMouseEvent.Button == MouseButton.Right)
        {
            if(DrawingToolsWindow.IsDragging)
                DrawingToolsWindow.EndDragWindow(e.Touches[0]);
        }
    }

    private PointF GetRelativePosition(System.Drawing.Point absolutePos)
    {
        if (_windowHandle == IntPtr.Zero)
            _windowHandle = Process.GetCurrentProcess().MainWindowHandle;

        System.Drawing.Point relativePos = absolutePos.RelativeToWindow(_windowHandle);
        return new PointF(relativePos.X, relativePos.Y);
    }
}