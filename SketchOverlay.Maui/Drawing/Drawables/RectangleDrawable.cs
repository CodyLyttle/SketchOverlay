namespace SketchOverlay.Maui.Drawing.Drawables;

internal class RectangleDrawable : MauiDrawing
{
    public MauiColor FillColor { get; set; } = Colors.Transparent;
    public MauiColor StrokeColor { get; set; } = Colors.Transparent;
    public float StrokeSize { get; set; } = 4;
    public PointF PointA { get; set; }
    public PointF PointB { get; set; }

    public void Draw(ICanvas canvas, RectF dirtyRect)
    {
        canvas.FillColor = FillColor;
        canvas.StrokeColor = StrokeColor;
        canvas.StrokeSize = StrokeSize;

        RectF rect = new(
            PointA.X, 
            PointA.Y, 
            PointB.X - PointA.X, 
            PointB.Y - PointA.Y);

        canvas.FillRectangle(rect);
        canvas.DrawRectangle(rect);
    }
}