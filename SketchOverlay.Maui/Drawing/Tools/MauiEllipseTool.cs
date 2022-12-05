using SketchOverlay.Library.Drawing.Canvas;
using SketchOverlay.Library.Drawing.Tools;
using SketchOverlay.Maui.Drawing.Drawables;
using PointF = System.Drawing.PointF;

namespace SketchOverlay.Maui.Drawing.Tools;

internal class MauiEllipseTool : DrawingTool<EllipseDrawable, MauiColor>, IEllipseTool<EllipseDrawable, MauiColor>
{
    protected override EllipseDrawable DoCreateDrawing(ICanvasProperties<Color> canvasProps, PointF startPoint)
    {
        EllipseDrawable drawable = base.DoCreateDrawing(canvasProps, startPoint);
        drawable.FillColor = canvasProps.FillColor;
        drawable.StrokeColor = canvasProps.StrokeColor;
        drawable.StrokeSize = canvasProps.StrokeSize;
        drawable.PointA = startPoint.ToMauiPointF();
        return drawable;
    }

    public override void DoUpdateDrawing(PointF currentPoint)
    {
        CurrentDrawing.PointB = currentPoint.ToMauiPointF();
    }
}