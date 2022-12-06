using System.Drawing;
using SketchOverlay.Library.Drawing.Canvas;

namespace SketchOverlay.Library.Drawing.Tools;

public abstract class DrawingTool<TDrawing, TColor> : IDrawingTool<TDrawing, TColor>
    where TDrawing : class, new()
{
    private TDrawing? _currentDrawing;
    public TDrawing CurrentDrawing
    {
        get => _currentDrawing ?? throw new InvalidOperationException($"{nameof(CurrentDrawing)} is null");
        private set => _currentDrawing = value;
    }

    public TDrawing CreateDrawing(ICanvasProperties<TColor> canvasProps, PointF startPoint)
    {
        if (_currentDrawing is not null)
            throw new InvalidOperationException(
                "Cannot create a new drawing before finishing the current drawing");

        CurrentDrawing = new TDrawing();
        InitializeDrawingProperties(canvasProps, startPoint);
        UpdateDrawing(startPoint);

        return CurrentDrawing;
    }

    protected virtual void InitializeDrawingProperties(ICanvasProperties<TColor> canvasProps, PointF startPoint)
    {
        // Do nothing.
    }

    public void UpdateDrawing(PointF currentPoint)
    {
        if (_currentDrawing is null)
            throw new InvalidOperationException(
                "Cannot update a drawing before creating a drawing");

        DoUpdateDrawing(currentPoint);
    }

    protected abstract void DoUpdateDrawing(PointF currentPoint);

    public void FinishDrawing()
    {
        if (_currentDrawing is null)
            throw new InvalidOperationException(
                "Cannot finish a drawing before creating a drawing");

        DoFinishDrawing();
        _currentDrawing = null;
    }

    protected virtual void DoFinishDrawing()
    {
        // Do nothing.
    }
}