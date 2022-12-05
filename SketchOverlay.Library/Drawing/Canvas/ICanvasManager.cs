﻿using System.Drawing;

namespace SketchOverlay.Library.Drawing.Canvas;

public interface ICanvasManager<out TOutput>
{
    TOutput DrawingOutput { get; }
    public bool IsDrawing { get; }

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