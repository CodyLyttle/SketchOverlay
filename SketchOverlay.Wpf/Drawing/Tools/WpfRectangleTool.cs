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

    protected override GeometryDrawing DoCreateDrawing(ICanvasProperties<WpfBrush> canvasProps, PointF startPoint)
    {
        Point wpfStartPoint = startPoint.ToWpfPoint();
        _rectangle = new RectangleGeometry(new Rect(wpfStartPoint, wpfStartPoint));

        GeometryDrawing drawing = base.DoCreateDrawing(canvasProps, startPoint);
        drawing.Geometry = _rectangle;
        drawing.Pen = canvasProps.StrokeColor.ToPen(canvasProps.StrokeSize);
        drawing.Brush = canvasProps.FillColor;
        return drawing;
    }

    public override void DoUpdateDrawing(PointF currentPoint)
    {
        _rectangle!.Rect = _rectangle.Rect with
        {
            Width = _rectangle.Rect.X - currentPoint.X,
            Height = _rectangle.Rect.Y - currentPoint.Y
        };
    }

    protected override void DoFinishDrawing()
    {
        base.DoFinishDrawing();
        _rectangle!.Freeze();
        _rectangle = null;
    }
}