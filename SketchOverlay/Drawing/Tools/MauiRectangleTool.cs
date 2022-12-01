using SketchOverlay.Drawing.Drawables;
using SketchOverlay.Library.Drawing;
using SketchOverlay.Library.Drawing.Tools;

namespace SketchOverlay.Drawing.Tools;

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