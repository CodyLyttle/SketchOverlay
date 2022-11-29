using SketchOverlay.Drawing.Drawables;
using SketchOverlay.Library.Drawing.Tools;
using SketchOverlay.LibraryAdapters;

namespace SketchOverlay.Drawing.Tools;

internal class MauiLineTool : DrawingTool<LineDrawable>, ILineTool<LineDrawable>
{
    public MauiLineTool(System.Drawing.Color strokeColor, float strokeSize)
    {
        StrokeColor = strokeColor;
        StrokeSize = strokeSize;
    }

    public System.Drawing.Color StrokeColor { get; set; }
    public float StrokeSize { get; set; }

    protected override LineDrawable DoCreateDrawing(System.Drawing.PointF startPoint)
    {
        LineDrawable drawable = base.DoCreateDrawing(startPoint);
        drawable.StrokeColor = StrokeColor.ToMauiColor();
        drawable.StrokeSize = StrokeSize;
        drawable.PointA = startPoint.ToMauiPointF();
        return drawable;
    }

    public override void UpdateDrawing(System.Drawing.PointF currentPoint)
    {
        CurrentDrawing.PointB = currentPoint.ToMauiPointF();
    }
}