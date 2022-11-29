using System.Drawing;

namespace SketchOverlay.Library.Drawing.Tools;

public abstract class DrawingTool<TDrawing> : IDrawingTool<TDrawing>
    where TDrawing : class, new()
{
    private TDrawing? _currentDrawing;
    public TDrawing CurrentDrawing
    {
        get => _currentDrawing ?? throw new InvalidOperationException($"{nameof(CurrentDrawing)} is null");
        private set => _currentDrawing = value;
    }

    public TDrawing CreateDrawing(PointF startPoint)
    {
        if (_currentDrawing is not null)
            throw new InvalidOperationException(
                "Cannot create a new drawing before finishing the current drawing");
        
        return DoCreateDrawing(startPoint);
    }

    protected virtual TDrawing DoCreateDrawing(PointF startPoint)
    {
        CurrentDrawing = new TDrawing();
        UpdateDrawing(startPoint);
        return CurrentDrawing;
    }

    public abstract void UpdateDrawing(PointF currentPoint);

    public void FinishDrawing()
    {
        DoFinishDrawing();
        _currentDrawing = null;
    }

    protected virtual void DoFinishDrawing()
    {
        // Do nothing.
    }
}