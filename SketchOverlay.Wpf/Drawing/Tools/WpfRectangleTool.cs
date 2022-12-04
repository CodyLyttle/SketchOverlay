using System.Drawing;
using System.Windows;
using System.Windows.Media;
using SketchOverlay.Library.Drawing.Canvas;
using SketchOverlay.Library.Drawing.Tools;
using Point = System.Windows.Point;

namespace SketchOverlay.Wpf.Drawing.Tools;

internal class WpfRectangleTool : DrawingTool<GeometryDrawing, WpfBrush>, IRectangleTool<GeometryDrawing, WpfBrush>
{
    private RectangleGeometry? _rectangle;
    private Point _startPoint;

    protected override GeometryDrawing DoCreateDrawing(ICanvasProperties<WpfBrush> canvasProps, PointF startPoint)
    {
        _startPoint = startPoint.ToWpfPoint();
        _rectangle = new RectangleGeometry(new Rect(_startPoint, _startPoint));

        GeometryDrawing drawing = base.DoCreateDrawing(canvasProps, startPoint);
        drawing.Geometry = _rectangle;
        drawing.Pen = canvasProps.StrokeColor.ToPen(canvasProps.StrokeSize);
        drawing.Brush = canvasProps.FillColor;
        return drawing;
    }

    public override void DoUpdateDrawing(PointF currentPoint)
    {
        _rectangle!.Rect = new Rect(_startPoint, currentPoint.ToWpfPoint());
    }

    protected override void DoFinishDrawing()
    {
        base.DoFinishDrawing();
        _rectangle!.Freeze();
        _rectangle = null;
    }
}