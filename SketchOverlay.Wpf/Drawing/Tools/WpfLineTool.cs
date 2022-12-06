using System.Drawing;
using System.Windows.Media;
using SketchOverlay.Library.Drawing.Canvas;
using SketchOverlay.Library.Drawing.Tools;
using Point = System.Windows.Point;

namespace SketchOverlay.Wpf.Drawing.Tools;

internal class WpfLineTool : DrawingTool<GeometryDrawing, WpfBrush>, ILineTool<GeometryDrawing, WpfBrush>
{
    private LineGeometry? _line;

    protected override void InitializeDrawingProperties(ICanvasProperties<WpfBrush> canvasProps, PointF startPoint)
    {
        Point wpfStartPoint = startPoint.ToWpfPoint();
        _line = new LineGeometry(wpfStartPoint, wpfStartPoint);

        CurrentDrawing.Geometry = _line;
        CurrentDrawing.Pen = canvasProps.StrokeColor.ToPen(canvasProps.StrokeSize);
    }

    protected override void DoUpdateDrawing(PointF currentPoint)
    {
        _line!.EndPoint = currentPoint.ToWpfPoint();
    }

    protected override void DoFinishDrawing()
    {
        base.DoFinishDrawing();
        _line!.Freeze();
        _line = null;
    }
}