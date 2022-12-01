using SketchOverlay.Drawing.Drawables;
using SketchOverlay.Library.Drawing.Tools;
using SketchOverlay.LibraryAdapters;

namespace SketchOverlay.Drawing.Tools;

internal class MauiPaintBrushTool : DrawingTool<PaintBrushDrawable>, IPaintBrushTool<PaintBrushDrawable, Color>
{
    public MauiPaintBrushTool(Color strokeColor, float strokeSize)
    {
        StrokeColor = strokeColor;
        StrokeSize = strokeSize;
    }

    public Color StrokeColor { get; set; }
    public float StrokeSize { get; set; }

    protected override PaintBrushDrawable DoCreateDrawing(System.Drawing.PointF startPoint)
    {
        PaintBrushDrawable drawable = base.DoCreateDrawing(startPoint);
        drawable.StrokeColor = StrokeColor;
        drawable.StrokeSize = StrokeSize;
        return drawable;
    }

    public override void UpdateDrawing(System.Drawing.PointF currentPoint)
    {
        CurrentDrawing.AddDrawingPoint(currentPoint.ToMauiPointF());
    }
}
