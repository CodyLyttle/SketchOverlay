using SketchOverlay.Canvas;

namespace SketchOverlay.DrawingTools;

internal class RectangleBrush : IDrawingTool
{
    private class RectangleBrushOutput : IDrawable
    {
        private readonly CanvasProperties _canvasProperties;

        public RectangleBrushOutput(CanvasProperties canvasProperties, PointF startPoint)
        {
            _canvasProperties = canvasProperties;
            PointB = PointA = startPoint;
        }

        public PointF PointA { get; }
        public PointF PointB { get; set; }

        public void Draw(ICanvas canvas, RectF dirtyRect)
        {
            canvas.SetProperties(_canvasProperties);
            canvas.DrawRectangle(
                PointA.X,
                PointA.Y,
                PointB.X - PointA.X,
                PointB.Y - PointA.Y);
        }
    }

    private RectangleBrushOutput? _currentOutput;

    public IDrawable BeginDraw(CanvasProperties canvasProperties, PointF startPoint)
    {
        _currentOutput = new RectangleBrushOutput(canvasProperties, startPoint);
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