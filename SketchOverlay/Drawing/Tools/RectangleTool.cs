using SketchOverlay.Library.Drawing;
using SketchOverlay.LibraryAdapters;

namespace SketchOverlay.Drawing.Tools;

internal class RectangleTool : IDrawingTool<IDrawable>
{
    private class RectangleToolOutput : IDrawable
    {
        public RectangleToolOutput(PointF startPoint)
        {
            PointB = PointA = startPoint;
        }

        public PointF PointA { get; }

        public PointF PointB { get; set; }

        public void Draw(ICanvas canvas, RectF dirtyRect)
        {
            canvas.DrawRectangle(
                PointA.X,
                PointA.Y,
                PointB.X - PointA.X,
                PointB.Y - PointA.Y);
        }
    }

    private RectangleToolOutput? _currentOutput;

    public IDrawable BeginDraw(System.Drawing.PointF startPoint)
    {
        _currentOutput = new RectangleToolOutput(startPoint.ToMauiPointF());
        return _currentOutput;
    }

    public void ContinueDraw(System.Drawing.PointF currentPoint)
    {
        if (_currentOutput is null)
            throw new InvalidOperationException($"{nameof(ContinueDraw)} was called before {nameof(BeginDraw)}");

        _currentOutput.PointB = currentPoint.ToMauiPointF();
    }

    public void EndDraw()
    {
        _currentOutput = null;
    }
}