using SketchOverlay.Library.Drawing.Canvas;
using SketchOverlay.Library.Drawing.Tools;
using SketchOverlay.Maui.Drawing.Drawables;

namespace SketchOverlay.Maui.Drawing.Tools;

internal class MauiLineTool : DrawingTool<LineDrawable, MauiColor>, ILineTool<LineDrawable, MauiColor>
{
    protected override LineDrawable DoCreateDrawing(ICanvasProperties<MauiColor> canvasProps, System.Drawing.PointF startPoint)
    {
        LineDrawable drawable = base.DoCreateDrawing(canvasProps, startPoint);
        drawable.StrokeColor = canvasProps.StrokeColor;
        drawable.StrokeSize = canvasProps.StrokeSize;
        drawable.PointA = startPoint.ToMauiPointF();
        return drawable;
    }

    public override void DoUpdateDrawing(System.Drawing.PointF currentPoint)
    {
        CurrentDrawing.PointB = currentPoint.ToMauiPointF();
    }
}