using SketchOverlay.Library.Drawing.Canvas;
using SketchOverlay.Library.Drawing.Tools;
using SketchOverlay.Maui.Drawing.Drawables;
using PointF = System.Drawing.PointF;

namespace SketchOverlay.Maui.Drawing.Tools;

internal class MauiEllipseTool : DrawingTool<EllipseDrawable, MauiColor>, IEllipseTool<EllipseDrawable, MauiColor>
{
    protected override void InitializeDrawingProperties(ICanvasProperties<Color> canvasProps, PointF startPoint)
    {
        CurrentDrawing.FillColor = canvasProps.FillColor;
        CurrentDrawing.StrokeColor = canvasProps.StrokeColor;
        CurrentDrawing.StrokeSize = canvasProps.StrokeSize;
        CurrentDrawing.PointA = startPoint.ToMauiPointF();
    }

    protected override void DoUpdateDrawing(PointF currentPoint)
    {
        CurrentDrawing.PointB = currentPoint.ToMauiPointF();
    }
}