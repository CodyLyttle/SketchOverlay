using System.Diagnostics;
using SketchOverlay.DrawingTools;
using SketchOverlay.Input;
using SketchOverlay.Native;
using SketchOverlay.Native.Input.Mouse;

namespace SketchOverlay;

public partial class MainPage : ContentPage
{
    private readonly DrawingCanvas _rootDrawable = new(new BrushTool());
    private readonly IInputManger _inputManager;
    private IntPtr _windowHandle;

    // TODO: Inject IInputManager via constructor & MauiProgram.cs
    public MainPage()
    {
        InitializeComponent();
        _inputManager = new InputManager();

        canvas.Drawable = _rootDrawable;
        canvas.DragInteraction += CanvasOnDragInteraction;
        canvas.EndInteraction += CanvasOnEndInteraction;

        clearButton.Clicked += (_, _) => _rootDrawable.Clear();
        redoButton.Clicked += (_, _) => _rootDrawable.Redo();
        undoButton.Clicked += (_, _) => _rootDrawable.Undo();

        redButton.Clicked += (_, _) => _rootDrawable.SetStrokeColor(Colors.Red);
        greenButton.Clicked += (_, _) => _rootDrawable.SetStrokeColor(Colors.Green);
        blueButton.Clicked += (_, _) => _rootDrawable.SetStrokeColor(Colors.Blue);

        brushTool.Clicked += (_, _) => _rootDrawable.DrawingTool = new BrushTool();
        lineTool.Clicked += (_, _) => _rootDrawable.DrawingTool = new LineTool();
        rectangleTool.Clicked += (_, _) => _rootDrawable.DrawingTool = new RectangleTool();

        drawSizeSlider.ValueChanged += (_, args) => _rootDrawable.SetStrokeSize((float)args.NewValue);

        _rootDrawable.RequestRedraw += (_, _) => canvas.Invalidate();
        _rootDrawable.CanClearChanged += (_, enabled) => UpdateButton(clearButton, enabled);
        _rootDrawable.CanRedoChanged += (_, enabled) => UpdateButton(redoButton, enabled);
        _rootDrawable.CanUndoChanged += (_, enabled) => UpdateButton(undoButton, enabled);

        clearButton.IsEnabled = false;
        redoButton.IsEnabled = false;
        undoButton.IsEnabled = false;
        _inputManager.Events.MouseMove += (_, args) => DisplayMouseMoveAction(args);
        _inputManager.Events.MouseUp += (_, args) => DisplayMouseButtonAction(args, "up");
        _inputManager.Events.MouseDown += (_, args) => DisplayMouseButtonAction(args, "down");
        _inputManager.Events.MouseWheel += (_, args) => DisplayMouseWheelAction(args);
    }

    private void DisplayMouseMoveAction(MouseMoveArgs e)
    {
        Dispatcher.Dispatch(() =>
            debugLabel.Text = $"Move\r\n{GetRelativePosition(e.Position)}"
        );
    }

    private void DisplayMouseButtonAction(MouseButtonEventArgs e, string action)
    {
        string debugTxt = e.Button == MouseButton.XButton
            ? $"{e.Button}{e.XButtonIndex} {action}\r\n{GetRelativePosition(e.Position)}"
            : $"{e.Button} {action}\r\n{GetRelativePosition(e.Position)}";

        Dispatcher.Dispatch(() => debugLabel.Text = debugTxt);
    }

    private void DisplayMouseWheelAction(MouseWheelArgs e)
    {
        Dispatcher.Dispatch(() => debugLabel.Text = $"Scroll {e.Direction}\r\n{GetRelativePosition(e.Position)}");
    }

    private void CanvasOnDragInteraction(object? sender, TouchEventArgs e)
    {
        // Mouse movement creates a single drawing event.
        _rootDrawable.DoDrawingEvent(e.Touches[0]);
    }

    private void CanvasOnEndInteraction(object? sender, TouchEventArgs e)
    {
        _rootDrawable.EndDrawingEvent();
    }

    private void UpdateButton(Button button, bool enabled)
    {
        Dispatcher.Dispatch(() => button.IsEnabled = enabled);
    }

    private System.Drawing.Point GetRelativePosition(System.Drawing.Point absolutePosition)
    {
        _windowHandle = Process.GetCurrentProcess().MainWindowHandle;
        return absolutePosition.RelativeToWindow(_windowHandle);
    }
}