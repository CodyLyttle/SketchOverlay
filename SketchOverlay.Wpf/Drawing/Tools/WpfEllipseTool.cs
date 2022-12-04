using System.Drawing;
using System.Windows;
using SketchOverlay.Library.Drawing.Tools;
using System.Windows.Media;
using SketchOverlay.Library.Drawing.Canvas;
using Point = System.Windows.Point;

namespace SketchOverlay.Wpf.Drawing.Tools;

internal class WpfEllipseTool : DrawingTool<GeometryDrawing, WpfBrush>, IEllipseTool<GeometryDrawing, WpfBrush>
{
    private EllipseGeometry? _ellipse;
    private Point _startPoint;

    protected override GeometryDrawing DoCreateDrawing(ICanvasProperties<WpfBrush> canvasProps, PointF startPoint)
    {
        _startPoint = startPoint.ToWpfPoint();
        _ellipse = new EllipseGeometry(new Rect(_startPoint, _startPoint));

        GeometryDrawing drawing = base.DoCreateDrawing(canvasProps, startPoint);
        drawing.Geometry = _ellipse;
        drawing.Pen = canvasProps.StrokeColor.ToPen(canvasProps.StrokeSize);
        drawing.Brush = canvasProps.FillColor;
        return drawing;
    }

    public override void DoUpdateDrawing(PointF currentPoint)
    {
        Point newPoint = currentPoint.ToWpfPoint();
        double radiusX = (newPoint.X - _startPoint.X) / 2;
        double radiusY = (newPoint.Y - _startPoint.Y) / 2;
        _ellipse!.RadiusX = radiusX;
        _ellipse.RadiusY = radiusY;
        _ellipse.Center = new Point(
            _startPoint.X + radiusX, 
            _startPoint.Y + radiusY);
    }

    protected override void DoFinishDrawing()
    {
        base.DoFinishDrawing();
        _ellipse!.Freeze();
        _ellipse = null;
    }
}