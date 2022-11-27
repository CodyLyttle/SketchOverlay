using System.Drawing;

namespace SketchOverlay.Library.Drawing;

public interface IDrawingTool<out TDrawing>
{
    TDrawing BeginDraw(PointF startPoint);
    void ContinueDraw(PointF currentPoint);
    void EndDraw();
}