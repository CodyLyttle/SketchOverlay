using SketchOverlay.DrawingTools;

namespace SketchOverlay;

internal class DrawingCanvas : IDrawable
{
    private bool _isDrawing;
    private readonly Stack<IDrawable> _drawables = new();

    public DrawingCanvas()
    {
        DrawingTool = new RectangleBrush();
    }

    public event EventHandler? RequestRedraw;

    public IDrawingTool DrawingTool { get; set; }


    public void Draw(ICanvas canvas, RectF dirtyRect)
    {
        foreach (IDrawable drawable in _drawables)
        {
            drawable.Draw(canvas, dirtyRect);
        }
    }

    public void ClearCanvas()
    {
        _drawables.Clear();
        Redraw();
    }

    public void DoDrawingEvent(PointF point)
    {
        if (!_isDrawing)
        {
            _isDrawing = true;
            _drawables.Push(DrawingTool.BeginDraw(point));
        }
        else
        {
            DrawingTool.ContinueDraw(point);
        }

        Redraw();
    }

    public void EndDrawingEvent()
    {
        _isDrawing = false;
        DrawingTool.EndDraw();
        Redraw();
    }

    private void Redraw()
    {
        RequestRedraw?.Invoke(this, EventArgs.Empty);
    }
}