namespace SketchOverlay;

public partial class MainPage : ContentPage
{
    private readonly DrawingCanvas _rootDrawable = new();

    public MainPage()
    {
        InitializeComponent();

        canvas.Drawable = _rootDrawable;
        canvas.DragInteraction += CanvasOnDragInteraction;
        canvas.EndInteraction += CanvasOnEndInteraction;
        clearButton.Clicked += (_,_) => _rootDrawable.ClearCanvas();
        _rootDrawable.RequestRedraw += (_, _) => canvas.Invalidate();
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
}

