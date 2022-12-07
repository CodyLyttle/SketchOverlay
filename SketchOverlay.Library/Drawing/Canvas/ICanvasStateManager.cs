namespace SketchOverlay.Library.Drawing.Canvas;

public interface ICanvasStateManager
{
    event EventHandler<bool>? CanClearChanged;
    event EventHandler<bool>? CanRedoChanged;
    event EventHandler<bool>? CanUndoChanged;

    bool CanUndo { get; }
    bool CanRedo { get; }
    bool CanClear { get; }

    void Undo();
    void Redo();
    void Clear();
}