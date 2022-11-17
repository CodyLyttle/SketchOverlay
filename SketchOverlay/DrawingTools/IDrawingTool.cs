namespace SketchOverlay.DrawingTools;

internal interface IDrawingTool
{
    IDrawable BeginDraw(PointF startPoint);
    void ContinueDraw(PointF currentPoint);
    void EndDraw();
}