using SketchOverlay.Library.Drawing.Canvas;

namespace SketchOverlay.Maui.Drawing;

public class DrawableStack : IDrawingStack<MauiDrawing, MauiDrawingOutput>, MauiDrawingOutput
{
    private readonly Stack<MauiDrawing> _drawables = new();

    public int Count => _drawables.Count;

    public MauiDrawingOutput Output => this;

    public void Clear()
    {
        _drawables.Clear();
    }

    public void PushDrawing(MauiDrawing drawing)
    {
        _drawables.Push(drawing);
    }

    public MauiDrawing PopDrawing()
    {
        return _drawables.Pop();
    }

    public void Draw(ICanvas canvas, RectF dirtyRect)
    {
        foreach (MauiDrawing drawable in _drawables.Reverse())
        {
            drawable.Draw(canvas, dirtyRect);
        }
    }
}