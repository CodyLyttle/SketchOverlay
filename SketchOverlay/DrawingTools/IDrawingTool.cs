using SketchOverlay.Canvas;

namespace SketchOverlay.DrawingTools;

internal interface IDrawingTool
{
    IDrawable BeginDraw(CanvasProperties canvasProperties, PointF startPoint);
    void ContinueDraw(PointF currentPoint);
    void EndDraw();
}