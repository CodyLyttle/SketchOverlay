using SketchOverlay.Canvas;

namespace SketchOverlay.DrawingTools;

internal class LineTool : IDrawingTool
{
    private class LineToolOutput : IDrawable
    {
        private readonly CanvasProperties _canvasProperties;

        public LineToolOutput(CanvasProperties canvasProperties, PointF startPoint)
        {
            _canvasProperties = canvasProperties;
            PointB = PointA = startPoint;
        }

        public PointF PointA { get; }
        public PointF PointB { get; set; }

        public void Draw(ICanvas canvas, RectF dirtyRect)
        {
            canvas.SetProperties(_canvasProperties);
            canvas.DrawLine(PointA, PointB);
        }
    }

    private LineToolOutput? _currentOutput;

    public IDrawable BeginDraw(CanvasProperties canvasProperties, PointF startPoint)
    {
        _currentOutput = new LineToolOutput(canvasProperties, startPoint);

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