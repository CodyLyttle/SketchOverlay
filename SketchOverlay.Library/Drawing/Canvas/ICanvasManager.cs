using System.Drawing;

namespace SketchOverlay.Library.Drawing.Canvas;

public interface ICanvasManager<out TOutput>
{
    bool CanUndo { get; }
    bool CanRedo { get; }
    bool CanClear { get; }
    bool IsDrawing { get; }
    TOutput DrawingOutput { get; }

    void Undo();
    void Redo();
    void Clear();

    void DoDrawing(PointF point);
    void FinishDrawing();
    void CancelDrawing();

    event EventHandler? RequestRedraw;
    event EventHandler<bool>? CanClearChanged;
    event EventHandler<bool>? CanRedoChanged;
    event EventHandler<bool>? CanUndoChanged;
}