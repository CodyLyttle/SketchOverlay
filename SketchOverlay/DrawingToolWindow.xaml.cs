using SketchOverlay.DrawingTools;

namespace SketchOverlay;

public partial class DrawingToolWindow : ContentView
{
    public DrawingToolWindow()
    {
        InitializeComponent();
        HideWindow();
    }

    public bool IsDragging { get; private set; }

    public void ShowWindow()
    {
        IsVisible = true;
    }

    public void BeginDragWindow(PointF position)
    {
        IsDragging = true;
        InputTransparent = true;
        MoveWindow(position);
    }

    public void ContinueDragWindow(PointF position)
    {
        if (!IsDragging)
            throw new InvalidOperationException(
                $"{nameof(ContinueDragWindow)} was called before {nameof(BeginDragWindow)}");

        MoveWindow(position);
    }

    public void EndDragWindow(PointF position)
    {
        if (!IsDragging)
            return;

        MoveWindow(position);
        InputTransparent = false;
        IsDragging = false;
    }

    public void HideWindow()
    {
        InputTransparent = false;
        IsVisible = false;
    }

    private void MoveWindow(PointF position)
    {
        // Width is -1 when called too early.
        double width = Width == -1
            ? WidthRequest
            : Width;

        double left = position.X - width / 2;
        double top = position.Y;
        Margin = new Thickness(left, top, 0, 0);
    }
}