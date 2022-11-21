using SketchOverlay.Drawing.Canvas;

namespace SketchOverlay.Drawing.Tools;

public interface IDrawingTool
{
    IDrawable BeginDraw(CanvasProperties canvasProperties, PointF startPoint);
    void ContinueDraw(PointF currentPoint);
    void EndDraw();
}