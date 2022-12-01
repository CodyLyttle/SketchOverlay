using SketchOverlay.Drawing.Drawables;
using SketchOverlay.Library.Drawing;
using SketchOverlay.Library.Drawing.Tools;

namespace SketchOverlay.Drawing.Tools;

internal class MauiPaintBrushTool : DrawingTool<PaintBrushDrawable, Color>, IPaintBrushTool<PaintBrushDrawable, Color>
{
    protected override PaintBrushDrawable DoCreateDrawing(ICanvasProperties<Color> canvasProps, System.Drawing.PointF startPoint)
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
