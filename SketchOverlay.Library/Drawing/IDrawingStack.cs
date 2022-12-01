namespace SketchOverlay.Library.Drawing;

public interface IDrawingStack<TDrawing, out TOutput>
{
    int Count { get; }
    TOutput Output { get; }

    void Clear();
    void PushDrawing(TDrawing drawing);
    TDrawing PopDrawing();
}