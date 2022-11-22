﻿using SketchOverlay.Drawing.Tools;

namespace SketchOverlay.Drawing.Canvas;

public interface IDrawingCanvas : IDrawable
{
    IDrawingTool DrawingTool { get; set; }
    Color StrokeColor { get; set; }
    float StrokeSize { get; set; }

    void Undo();
    void Redo();
    void Clear();

    void DoDrawingEvent(PointF point);
    void FinalizeDrawingEvent();
    void CancelDrawingEvent();
}