using SketchOverlay.Library.Drawing.Canvas;

namespace SketchOverlay.Maui.Drawing;

public class DrawableStack : IDrawingStack<IDrawable, IDrawable>, IDrawable
{
    private readonly Stack<IDrawable> _drawables = new();

    public int Count => _drawables.Count;

    public IDrawable Output => this;

    public void Clear()
    {
        _drawables.Clear();
    }

    public void PushDrawing(IDrawable drawing)
    {
        _drawables.Push(drawing);
    }

    public IDrawable PopDrawing()
    {
        return _drawables.Pop();
    }

    public void Draw(ICanvas canvas, RectF dirtyRect)
    {
        foreach (IDrawable drawable in _drawables.Reverse())
        {
            drawable.Draw(canvas, dirtyRect);
        }
    }
}