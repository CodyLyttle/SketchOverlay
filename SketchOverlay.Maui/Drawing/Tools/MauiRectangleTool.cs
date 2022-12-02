using SketchOverlay.Library.Drawing.Canvas;
using SketchOverlay.Library.Drawing.Tools;
using SketchOverlay.Maui.Drawing.Drawables;

namespace SketchOverlay.Maui.Drawing.Tools;

internal class MauiRectangleTool : DrawingTool<RectangleDrawable, Color>, IRectangleTool<RectangleDrawable, Color>
{
    protected override RectangleDrawable DoCreateDrawing(ICanvasProperties<Color> canvasProps, System.Drawing.PointF startPoint)
    {
        RectangleDrawable drawable =  base.DoCreateDrawing(canvasProps, startPoint);
        drawable.FillColor = canvasProps.FillColor;
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