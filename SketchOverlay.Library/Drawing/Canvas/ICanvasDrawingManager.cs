using System.Drawing;

namespace SketchOverlay.Library.Drawing.Canvas;

public interface ICanvasDrawingManager<out TOutput>
{
    event EventHandler? RequestRedraw;

    TOutput DrawingOutput { get; }
    bool IsDrawing { get; }

    void DoDrawing(PointF point);
    void FinishDrawing();
    void CancelDrawing();
}