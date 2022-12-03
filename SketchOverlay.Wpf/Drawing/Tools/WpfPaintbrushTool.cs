using System;
using System.Drawing;
using System.Windows.Media;
using SketchOverlay.Library.Drawing.Canvas;
using SketchOverlay.Library.Drawing.Tools;

namespace SketchOverlay.Wpf.Drawing.Tools;

internal class WpfPaintbrushTool : DrawingTool<GeometryDrawing, WpfBrush>, IPaintBrushTool<GeometryDrawing, WpfBrush>
{
    private PathGeometry? _brushPath;

    protected override GeometryDrawing DoCreateDrawing(ICanvasProperties<WpfBrush> canvasProps, PointF startPoint)
    {
        _brushPath = new PathGeometry();
        _brushPath.Figures.Add(new PathFigure(startPoint.ToWpfPoint(), Array.Empty<PathSegment>(), false));

        GeometryDrawing drawing = base.DoCreateDrawing(canvasProps, startPoint);
        drawing.Geometry = _brushPath;
        drawing.Pen = canvasProps.StrokeColor.ToPen(canvasProps.StrokeSize);
        return drawing;
    }

    public override void DoUpdateDrawing(PointF currentPoint)
    {
        // TODO: Optimize drawing.
        _brushPath!.Figures[0].Segments.Add(new LineSegment(currentPoint.ToWpfPoint(), true));
    }

    protected override void DoFinishDrawing()
    {
        base.DoFinishDrawing();
        _brushPath!.Freeze();
        _brushPath = null;
    }
}