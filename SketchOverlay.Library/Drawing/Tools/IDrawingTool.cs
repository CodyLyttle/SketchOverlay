using System.Drawing;
using SketchOverlay.Library.Drawing.Canvas;

namespace SketchOverlay.Library.Drawing.Tools;

public interface IDrawingTool<out TDrawing, TColor>
{
    TDrawing CurrentDrawing { get; }
    TDrawing CreateDrawing(ICanvasProperties<TColor> canvasProps, PointF startPoint);
    void UpdateDrawing(PointF currentPoint);
    void FinishDrawing();
}