namespace SketchOverlay.DrawingTools;

internal class RectangleBrush : IDrawingTool
{
    private class RectangleBrushOutput : IDrawable
    {
        public PointF PointA { get; init; }
        public PointF PointB { get; set; }

        public void Draw(ICanvas canvas, RectF dirtyRect)
        {
            canvas.StrokeColor = Colors.Red;

            canvas.DrawRectangle(
                PointA.X,
                PointA.Y,
                PointB.X - PointA.X,
                PointB.Y - PointA.Y);
        }
    }

    private RectangleBrushOutput? _currentOutput;

    public IDrawable BeginDraw(PointF startPoint)
    {
        _currentOutput = new RectangleBrushOutput
        {
            PointA = startPoint,
            PointB = startPoint
        };

        return _currentOutput;
    }

    public void ContinueDraw(PointF currentPoint)
    {
        if (_currentOutput is null)
            throw new InvalidOperationException($"{nameof(ContinueDraw)} was called before {nameof(BeginDraw)}");

        _currentOutput.PointB = currentPoint;
    }

    public void EndDraw()
    {
        _currentOutput = null;
    }
}