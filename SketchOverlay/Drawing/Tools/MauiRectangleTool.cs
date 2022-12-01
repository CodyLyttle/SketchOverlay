using SketchOverlay.Drawing.Drawables;
using SketchOverlay.Library.Drawing.Tools;
using SketchOverlay.LibraryAdapters;

namespace SketchOverlay.Drawing.Tools;

internal class MauiRectangleTool : DrawingTool<RectangleDrawable>, IRectangleTool<RectangleDrawable, Color>
{
    public MauiRectangleTool(Color fillColor, Color strokeColor, float strokeSize)
    {
        FillColor = fillColor;
        StrokeColor = strokeColor;
        StrokeSize = strokeSize;
    }

    public Color FillColor { get; set; }
    public Color StrokeColor { get; set; }
    public float StrokeSize { get; set; }

    protected override RectangleDrawable DoCreateDrawing(System.Drawing.PointF startPoint)
    {
        RectangleDrawable drawable =  base.DoCreateDrawing(startPoint);
        drawable.FillColor = FillColor;
        drawable.StrokeColor = StrokeColor;
        drawable.StrokeSize = StrokeSize;
        drawable.PointA = startPoint.ToMauiPointF();
        return drawable;
    }

    public override void UpdateDrawing(System.Drawing.PointF currentPoint)
    {
        CurrentDrawing.PointB = currentPoint.ToMauiPointF();
    }
}