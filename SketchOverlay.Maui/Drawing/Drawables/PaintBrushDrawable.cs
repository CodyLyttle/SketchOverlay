namespace SketchOverlay.Maui.Drawing.Drawables;

internal class PaintBrushDrawable : MauiDrawing
{
    private readonly PathF _brushPath;
    
    public PaintBrushDrawable()
    {
        _brushPath = new PathF();
    }

    public MauiColor StrokeColor { get; set; } = Colors.Gray;
    public float StrokeSize { get; set; } = 4;

    public void AddDrawingPoint(PointF point)
    {
        _brushPath.LineTo(point);
    }

    public void Draw(ICanvas canvas, RectF dirtyRect)
    {
        canvas.StrokeColor = StrokeColor;
        canvas.StrokeLineCap = LineCap.Round;
        canvas.StrokeLineJoin = LineJoin.Round;
        canvas.StrokeSize = StrokeSize;
        canvas.DrawPath(_brushPath);
    }
}