using SketchOverlay.Drawing.Tools;

namespace SketchOverlay.Drawing.Canvas;

public interface IDrawingCanvas : IDrawable
{
    IDrawingTool DrawingTool { set; }
    Color StrokeColor { set; }
    float StrokeSize { set; }

    void Undo();
    void Redo();
    void Clear();

    void DoDrawingEvent(PointF point);
    void FinalizeDrawingEvent();
    void CancelDrawingEvent();
}