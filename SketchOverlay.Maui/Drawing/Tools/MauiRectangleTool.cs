using SketchOverlay.Library.Drawing.Canvas;
using SketchOverlay.Library.Drawing.Tools;
using SketchOverlay.Maui.Drawing.Drawables;

namespace SketchOverlay.Maui.Drawing.Tools;

internal class MauiRectangleTool : DrawingTool<RectangleDrawable, MauiColor>, IRectangleTool<RectangleDrawable, MauiColor>
{
    protected override void InitializeDrawingProperties(ICanvasProperties<MauiColor> canvasProps, System.Drawing.PointF startPoint)
    {
        CurrentDrawing.FillColor = canvasProps.FillColor;
        CurrentDrawing.StrokeColor = canvasProps.StrokeColor;
        CurrentDrawing.StrokeSize = canvasProps.StrokeSize;
        CurrentDrawing.PointA = startPoint.ToMauiPointF();
    }

    protected override void DoUpdateDrawing(System.Drawing.PointF currentPoint)
    {
        CurrentDrawing.PointB = currentPoint.ToMauiPointF();
    }
}