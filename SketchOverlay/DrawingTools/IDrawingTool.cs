using SketchOverlay.Canvas;

namespace SketchOverlay.DrawingTools;

public interface IDrawingTool
{
    IDrawable BeginDraw(CanvasProperties canvasProperties, PointF startPoint);
    void ContinueDraw(PointF currentPoint);
    void EndDraw();
}