namespace SketchOverlay.Maui.Drawing.Drawables;

internal class LineDrawable : MauiDrawing
{
    public MauiColor StrokeColor { get; set; } = Colors.Gray;
    public float StrokeSize { get; set; } = 4;
    public PointF PointA { get; set; }
    public PointF PointB { get; set; }

    public void Draw(ICanvas canvas, RectF dirtyRect)
    {
        canvas.StrokeColor = StrokeColor;
        canvas.StrokeSize = StrokeSize;
        canvas.DrawLine(PointA, PointB);
    }
}