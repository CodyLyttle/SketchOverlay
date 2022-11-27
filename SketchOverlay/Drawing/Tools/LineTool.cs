using SketchOverlay.Library.Drawing;
using SketchOverlay.LibraryAdapters;

namespace SketchOverlay.Drawing.Tools;

internal class LineTool : IDrawingTool<IDrawable>
{
    private class LineToolOutput : IDrawable
    {
        public LineToolOutput(PointF startPoint)
        {
            PointB = PointA = startPoint;
        }

        public PointF PointA { get; }

        public PointF PointB { get; set; }

        public void Draw(ICanvas canvas, RectF dirtyRect)
        {
            canvas.DrawLine(PointA, PointB);
        }
    }

    private LineToolOutput? _currentOutput;

    public IDrawable BeginDraw(System.Drawing.PointF startPoint)
    {
        _currentOutput = new LineToolOutput(startPoint.ToMauiPointF());
        return _currentOutput;
    }

    public void ContinueDraw(System.Drawing.PointF currentPoint)
    {
        if(_currentOutput == null)
            throw new InvalidOperationException($"{nameof(ContinueDraw)} was called before {nameof(BeginDraw)}");

        _currentOutput.PointB = currentPoint.ToMauiPointF();
    }

    public void EndDraw()
    {
        _currentOutput = null;
    }
}