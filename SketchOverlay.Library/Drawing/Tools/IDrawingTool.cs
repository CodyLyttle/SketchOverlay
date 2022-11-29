using System.Drawing;

namespace SketchOverlay.Library.Drawing.Tools;

public interface IDrawingTool<out TDrawing>
{
    TDrawing CreateDrawing(PointF startPoint);
    void UpdateDrawing(PointF currentPoint);
    void FinishDrawing();
}