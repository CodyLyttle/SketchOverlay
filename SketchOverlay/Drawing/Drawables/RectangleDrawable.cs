namespace SketchOverlay.Drawing.Drawables;

internal class RectangleDrawable : IDrawable
{
    public Color? FillColor { get; set; } = null;
    public Color StrokeColor { get; set; } = Colors.Gray;
    public float StrokeSize { get; set; } = 4;
    public PointF PointA { get; set; }
    public PointF PointB { get; set; }

    public void Draw(ICanvas canvas, RectF dirtyRect)
    {
        canvas.FillColor = FillColor;
        canvas.StrokeColor = StrokeColor;
        canvas.StrokeSize = StrokeSize;
        canvas.DrawRectangle(
            PointA.X,
            PointA.Y,
            PointB.X - PointA.X,
            PointB.Y - PointA.Y);
    }
}