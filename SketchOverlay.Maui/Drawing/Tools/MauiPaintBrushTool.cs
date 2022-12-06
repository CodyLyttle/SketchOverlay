using SketchOverlay.Library.Drawing.Canvas;
using SketchOverlay.Library.Drawing.Tools;
using SketchOverlay.Maui.Drawing.Drawables;

namespace SketchOverlay.Maui.Drawing.Tools;

internal class MauiPaintBrushTool : DrawingTool<PaintBrushDrawable, MauiColor>, IPaintBrushTool<PaintBrushDrawable, MauiColor>
{
    protected override void InitializeDrawingProperties(ICanvasProperties<MauiColor> canvasProps, System.Drawing.PointF startPoint)
    {
        CurrentDrawing.StrokeColor = canvasProps.StrokeColor;
        CurrentDrawing.StrokeSize = canvasProps.StrokeSize;
    }

    protected override void DoUpdateDrawing(System.Drawing.PointF currentPoint)
    {
        CurrentDrawing.AddDrawingPoint(currentPoint.ToMauiPointF());
    }
}
