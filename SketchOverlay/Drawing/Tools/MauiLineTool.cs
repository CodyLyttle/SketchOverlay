using SketchOverlay.Drawing.Drawables;
using SketchOverlay.Library.Drawing;
using SketchOverlay.Library.Drawing.Tools;

namespace SketchOverlay.Drawing.Tools;

internal class MauiLineTool : DrawingTool<LineDrawable, Color>, ILineTool<LineDrawable, Color>
{
    protected override LineDrawable DoCreateDrawing(ICanvasProperties<Color> canvasProps, System.Drawing.PointF startPoint)
    {
        LineDrawable drawable = base.DoCreateDrawing(canvasProps, startPoint);
        drawable.StrokeColor = canvasProps.StrokeColor;
        drawable.StrokeSize = canvasProps.StrokeSize;
        drawable.PointA = startPoint.ToMauiPointF();
        return drawable;
    }

    public override void UpdateDrawing(System.Drawing.PointF currentPoint)
    {
        CurrentDrawing.PointB = currentPoint.ToMauiPointF();
    }
}