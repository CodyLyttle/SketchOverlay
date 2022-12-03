using SketchOverlay.Library.Drawing.Canvas;
using SketchOverlay.Library.Drawing.Tools;
using SketchOverlay.Maui.Drawing.Drawables;

namespace SketchOverlay.Maui.Drawing.Tools;

internal class MauiPaintBrushTool : DrawingTool<PaintBrushDrawable, MauiColor>, IPaintBrushTool<PaintBrushDrawable, MauiColor>
{
    protected override PaintBrushDrawable DoCreateDrawing(ICanvasProperties<MauiColor> canvasProps, System.Drawing.PointF startPoint)
    {
        PaintBrushDrawable drawable = base.DoCreateDrawing(canvasProps, startPoint);
        drawable.StrokeColor = canvasProps.StrokeColor;
        drawable.StrokeSize = canvasProps.StrokeSize;
        return drawable;
    }

    public override void UpdateDrawing(System.Drawing.PointF currentPoint)
    {
        CurrentDrawing.AddDrawingPoint(currentPoint.ToMauiPointF());
    }
}
