using SketchOverlay.Library.Drawing;
using SketchOverlay.LibraryAdapters;

namespace SketchOverlay.Drawing.Tools;

internal class BrushTool : IDrawingTool<IDrawable>
{
    private class BrushToolOutput : IDrawable
    {
        private readonly PathF _brushPath;

        public BrushToolOutput(PointF startPoint)
        {
            _brushPath = new PathF(startPoint);
        }

        public void AddBrushPoint(PointF point)
        {
            _brushPath.LineTo(point);
        }

        public void Draw(ICanvas canvas, RectF dirtyRect)
        {
            canvas.StrokeSize = 4;
            canvas.StrokeColor = Colors.Red;
            canvas.StrokeLineCap = LineCap.Round;
            canvas.StrokeLineJoin = LineJoin.Round;
            canvas.DrawPath(_brushPath);
        }
    }

    private BrushToolOutput? _currentOutput;

    public IDrawable BeginDraw(System.Drawing.PointF startPoint)
    {
        _currentOutput = new BrushToolOutput(startPoint.ToMauiPointF());
        return _currentOutput;
    }

    public void ContinueDraw(System.Drawing.PointF currentPoint)
    {
        if (_currentOutput == null)
            throw new InvalidOperationException($"{nameof(ContinueDraw)} was called before {nameof(BeginDraw)}");

        _currentOutput.AddBrushPoint(currentPoint.ToMauiPointF());
    }

    public void EndDraw()
    {
        _currentOutput = null;
    }
}
