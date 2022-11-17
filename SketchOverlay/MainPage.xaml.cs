using SketchOverlay.DrawingTools;

namespace SketchOverlay;

public partial class MainPage : ContentPage
{
    private readonly DrawingCanvas _rootDrawable = new(new LineBrush());

    public MainPage()
    {
        InitializeComponent();

        canvas.Drawable = _rootDrawable;
        canvas.DragInteraction += CanvasOnDragInteraction;
        canvas.EndInteraction += CanvasOnEndInteraction;

        clearButton.Clicked += (_, _) => _rootDrawable.Clear();
        redoButton.Clicked += (_, _) => _rootDrawable.Redo();
        undoButton.Clicked += (_, _) => _rootDrawable.Undo();

        redButton.Clicked += (_, _) => _rootDrawable.SetStrokeColor(Colors.Red);
        greenButton.Clicked += (_, _) => _rootDrawable.SetStrokeColor(Colors.Green);
        blueButton.Clicked += (_, _) => _rootDrawable.SetStrokeColor(Colors.Blue);

        lineBrush.Clicked += (_, _) => _rootDrawable.DrawingTool = new LineBrush();
        rectangleBrush.Clicked += (_, _) => _rootDrawable.DrawingTool = new RectangleBrush();

        brushSizeSlider.ValueChanged += (_, args) => _rootDrawable.SetStrokeSize((float)args.NewValue);

        _rootDrawable.RequestRedraw += (_, _) => canvas.Invalidate();
        _rootDrawable.CanClearChanged += (_, enabled) => UpdateButton(clearButton, enabled);
        _rootDrawable.CanRedoChanged += (_, enabled) => UpdateButton(redoButton, enabled);
        _rootDrawable.CanUndoChanged += (_, enabled) => UpdateButton(undoButton, enabled);

        clearButton.IsEnabled = false;
        redoButton.IsEnabled = false;
        undoButton.IsEnabled = false;
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
}