using SketchOverlay.Drawing.Canvas;

namespace SketchOverlay.Drawing.Tools;

internal class BrushTool : IDrawingTool
{
    // Is there a better way to do this? 
    // Large brush size causes inaccurate lines to be drawn on sharp angles.
    private class BrushToolOutput : IDrawable
    {
        private readonly PathF _brushPath;
        private readonly CanvasProperties _canvasProperties;

        public BrushToolOutput(CanvasProperties canvasProperties, PointF startPoint)
        {
            _canvasProperties = canvasProperties;
            _brushPath = new PathF(startPoint);
        }

        public void AddBrushPoint(PointF point)
        {
            _brushPath.LineTo(point);
        }

        public void Draw(ICanvas canvas, RectF dirtyRect)
        {
            canvas.SetProperties(_canvasProperties);
            canvas.DrawPath(_brushPath);
        }
    }

    private BrushToolOutput? _currentOutput;

    public IDrawable BeginDraw(CanvasProperties canvasProperties, PointF startPoint)
    {
        _currentOutput = new BrushToolOutput(canvasProperties, startPoint);
        return _currentOutput;
    }

    public void ContinueDraw(PointF currentPoint)
    {
        if (_currentOutput == null)
            throw new InvalidOperationException($"{nameof(ContinueDraw)} was called before {nameof(BeginDraw)}");

        _currentOutput.AddBrushPoint(currentPoint);
    }

    public void EndDraw()
    {
        _currentOutput = null;
    }
}
