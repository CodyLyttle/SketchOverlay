using System;
using System.Drawing;
using System.Linq;
using System.Windows.Media;
using SketchOverlay.Library.Drawing.Canvas;
using SketchOverlay.Library.Drawing.Tools;
using Point = System.Windows.Point;

namespace SketchOverlay.Wpf.Drawing.Tools;

internal class WpfPaintbrushTool : DrawingTool<GeometryDrawing, WpfBrush>, IPaintBrushTool<GeometryDrawing, WpfBrush>
{
    private PathGeometry? _brushPath;
    private Point _parentPoint;
    private Point _grandParentPoint;
    private bool _canSimplify;

    protected override void InitializeDrawingProperties(ICanvasProperties<WpfBrush> canvasProps, PointF startPoint)
    {
        _brushPath = new PathGeometry();
        _brushPath.Figures.Add(new PathFigure(startPoint.ToWpfPoint(), Array.Empty<PathSegment>(), false));
        _parentPoint = new Point();
        _grandParentPoint = new Point();
        _canSimplify = false;

        CurrentDrawing.Geometry = _brushPath;
        CurrentDrawing.Pen = CreatePen(canvasProps.StrokeColor, canvasProps.StrokeSize);
    }

    private System.Windows.Media.Pen CreatePen(WpfBrush brush, float strokeSize)
    {
        return new System.Windows.Media.Pen(brush, strokeSize)
        {
            LineJoin = PenLineJoin.Round,
            StartLineCap = PenLineCap.Round,
            EndLineCap = PenLineCap.Round
        };
    }

    protected override void DoUpdateDrawing(PointF currentPoint)
    {
        Point childPoint = currentPoint.ToWpfPoint();
        PathSegmentCollection segments = _brushPath!.Figures[0].Segments;
        var wasSimplified = false;

        if (_canSimplify)
        {
            wasSimplified = TrySimplify(childPoint, segments);
        }
        else if (segments.Count >= 2)
        {
            _canSimplify = true;
            wasSimplified = TrySimplify(childPoint, segments);
        }

        if (!wasSimplified)
        {
            segments.Add(new LineSegment(currentPoint.ToWpfPoint(), true));
        }

        _grandParentPoint = _parentPoint;
        _parentPoint = childPoint;
    }

    // Simplify path using Visvalingam’s algorithm
    // See: https://bost.ocks.org/mike/simplify/
    private bool TrySimplify(Point childPoint, PathSegmentCollection segments)
    {
        // Find the area of the triangle formed by the 3 most recent points.
        double sideA = Point.Subtract(_grandParentPoint, _parentPoint).Length;
        double sideB = Point.Subtract(_parentPoint, childPoint).Length;
        double sideC = Point.Subtract(_grandParentPoint, childPoint).Length;
        double semiPerimeter = (sideA + sideB + sideC) / 2;
        double triangleArea = Math.Sqrt(semiPerimeter * (semiPerimeter - sideA)
                                                      * (semiPerimeter - sideB)
                                                      * (semiPerimeter - sideC));

        // Determine if the area is small enough to simplify.
        // A value of 0.1 removes roughly 50% of all points.
        // Increasing the value has diminishing returns at the cost of drawing accuracy.
        if (triangleArea < 0.1)
        {
            // Replace parent point with child point.
            ((LineSegment)segments.Last()).Point = childPoint;
            return true;
        }

        return false;
    }

    protected override void DoFinishDrawing()
    {
        base.DoFinishDrawing();
        _brushPath!.Freeze();
        _brushPath = null;
    }
}