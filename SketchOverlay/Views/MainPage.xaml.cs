using System.Diagnostics;
using SketchOverlay.DrawingTools;
using SketchOverlay.Input;
using SketchOverlay.Native;
using SketchOverlay.Native.Input.Mouse;

namespace SketchOverlay.Views;

// TODO: Use InputManager.Events to handle the tool window dragging logic. Dragging stops abruptly due to the cursor leaving the bounds of the GraphicsView.
public partial class MainPage : ContentPage
{
    private const MouseButton DrawingButton = MouseButton.Left;
    private const MouseButton CancelDrawingButton = MouseButton.Right;
    private const MouseButton ToggleMenuButton = MouseButton.Middle;

    private readonly DrawingCanvas _drawingCanvas = new(new BrushTool());
    private readonly IInputManger _inputManager = new InputManager();
    private IntPtr _windowHandle;

    // TODO: Inject IInputManager via constructor & MauiProgram.cs
    public MainPage()
    {
        InitializeComponent();

        graphicsView.Drawable = _drawingCanvas;
        graphicsView.StartInteraction += GraphicsViewOnStartInteraction;
        graphicsView.DragInteraction += GraphicsViewOnDragInteraction;
        graphicsView.EndInteraction += GraphicsViewOnEndInteraction;
        _drawingCanvas.RequestRedraw += (_, _) => graphicsView.Invalidate();

        DrawingToolsWindow.PrimaryToolChanged += (_, tool) => _drawingCanvas.PrimaryDrawingTool = tool;
        DrawingToolsWindow.PrimaryToolColorChanged += (_, color) => _drawingCanvas.SetPrimaryStrokeColor(color);
        DrawingToolsWindow.PrimaryToolDrawSizeChanged += (_, size) => _drawingCanvas.SetPrimaryStrokeSize((float)size);

        DrawingToolsWindow.RequestUndo += (_, _) => _drawingCanvas.Undo();
        DrawingToolsWindow.RequestRedo += (_, _) => _drawingCanvas.Redo();
        DrawingToolsWindow.RequestClear += (_, _) => _drawingCanvas.Clear();
        _drawingCanvas.CanUndoChanged += (_, isEnabled) => DrawingToolsWindow.SetCanUndo(isEnabled);
        _drawingCanvas.CanRedoChanged += (_, isEnabled) => DrawingToolsWindow.SetCanRedo(isEnabled);
        _drawingCanvas.CanClearChanged += (_, isEnabled) => DrawingToolsWindow.SetCanClear(isEnabled);

        _inputManager.Events.MouseDown += OnMouseDown;
    }

    private void GraphicsViewOnStartInteraction(object? sender, TouchEventArgs e)
    {
        MouseButton lastMouseButton = _inputManager.State.LastMouseButtonEvent!.Button;

        if (lastMouseButton is DrawingButton)
        {
            // Allow drawing to continue when the cursor enters the drawing tool window.
            DrawingToolsWindow.InputTransparent = true;
        }
        else if (lastMouseButton is ToggleMenuButton)
        {
            if(DrawingToolsWindow.IsVisible)
                DrawingToolsWindow.HideWindow();
            else
            {
                DrawingToolsWindow.BeginDragWindow(e.Touches[0]);
                DrawingToolsWindow.ShowWindow();
            }
        }

        GraphicsViewOnDragInteraction(sender, e);
    }

    private void GraphicsViewOnDragInteraction(object? sender, TouchEventArgs e)
    {
        PointF cursorPos = e.Touches[0];
        MouseButton lastMouseButton = _inputManager.State.LastMouseButtonEvent!.Button;

        if(lastMouseButton is DrawingButton)
        {
            _drawingCanvas.DoPrimaryDrawingEvent(cursorPos);
        }
        else if (lastMouseButton is ToggleMenuButton && DrawingToolsWindow.IsDragging)
        {
            DrawingToolsWindow.ContinueDragWindow(cursorPos);
        }
    }

    private void GraphicsViewOnEndInteraction(object? sender, TouchEventArgs e)
    {
        MouseButton lastMouseButton = _inputManager.State.LastMouseButtonEvent!.Button;

        if (lastMouseButton is DrawingButton)
        {
            _drawingCanvas.EndDrawingEvent();
            DrawingToolsWindow.InputTransparent = false;
        }
        else if (lastMouseButton is ToggleMenuButton)
        {
            if (DrawingToolsWindow.IsDragging)
                DrawingToolsWindow.EndDragWindow(e.Touches[0]);
        }
    }

    private void OnMouseDown(object? sender, MouseButtonEventArgs args)
    {
        if (args.Button is CancelDrawingButton)
            _drawingCanvas.CancelPrimaryDrawingEvent();
    }

    // Will likely come in handy. Move to a relevant class.
    private PointF GetRelativePosition(System.Drawing.Point absolutePos)
    {
        if (_windowHandle == IntPtr.Zero)
            _windowHandle = Process.GetCurrentProcess().MainWindowHandle;

        System.Drawing.Point relativePos = absolutePos.RelativeToWindow(_windowHandle);
        return new PointF(relativePos.X, relativePos.Y);
    }
}