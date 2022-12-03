using System.Drawing;
using System.Windows.Media;
using SketchOverlay.Library.Drawing.Canvas;
using SketchOverlay.Library.Drawing.Tools;
using Point = System.Windows.Point;

namespace SketchOverlay.Wpf.Drawing.Tools;

internal class WpfLineTool : DrawingTool<GeometryDrawing, WpfBrush>, ILineTool<GeometryDrawing, WpfBrush>
{
    private LineGeometry? _line;

    protected override GeometryDrawing DoCreateDrawing(ICanvasProperties<WpfBrush> canvasProps, PointF startPoint)
    {
        Point wpfStartPoint = startPoint.ToWpfPoint();
        _line = new LineGeometry(wpfStartPoint, wpfStartPoint);

        GeometryDrawing drawing = base.DoCreateDrawing(canvasProps, startPoint);
        drawing.Geometry = _line;
        drawing.Pen = canvasProps.StrokeColor.ToPen(canvasProps.StrokeSize);
        return drawing;
    }

    public override void DoUpdateDrawing(PointF currentPoint)
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