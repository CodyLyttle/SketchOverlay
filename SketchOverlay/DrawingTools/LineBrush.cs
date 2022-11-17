namespace SketchOverlay.DrawingTools;

internal class LineBrush : IDrawingTool
{
    private class LineBrushOutput : IDrawable
    {
        public PointF PointA { get; init; }
        public PointF PointB { get; set; }

        public void Draw(ICanvas canvas, RectF dirtyRect)
        {
            canvas.StrokeColor = Colors.Red;
            canvas.DrawLine(PointA, PointB);
        }
    }

    private LineBrushOutput? _currentOutput;

    public IDrawable BeginDraw(PointF startPoint)
    {
        _currentOutput = new LineBrushOutput
        {
            PointA = startPoint,
            PointB = startPoint
        };

        return _currentOutput;
    }

    public void ContinueDraw(PointF currentPoint)
    {
        if(_currentOutput == null)
            throw new InvalidOperationException($"{nameof(ContinueDraw)} was called before {nameof(BeginDraw)}");

        _currentOutput.PointB = currentPoint;
    }

    public void EndDraw()
    {
        _currentOutput = null;
    }
}