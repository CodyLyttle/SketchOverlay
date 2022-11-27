﻿using System.Drawing;

namespace SketchOverlay.Library.Drawing;

public interface IDrawingCanvas<TDrawing>
{
    IDrawingTool<TDrawing> DrawingTool { get; set; }

    void Undo();
    void Redo();
    void Clear();

    void DoDrawingEvent(PointF point);
    void FinalizeDrawingEvent();
    void CancelDrawingEvent();

    event EventHandler? RequestRedraw;
    event EventHandler<bool>? CanClearChanged;
    event EventHandler<bool>? CanRedoChanged;
    event EventHandler<bool>? CanUndoChanged;
}