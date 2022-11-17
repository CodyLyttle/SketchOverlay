using SketchOverlay.Canvas;
using SketchOverlay.DrawingTools;

namespace SketchOverlay;

internal class DrawingCanvas : IDrawable
{
    private bool _isDrawing;
    private bool _canRedo;
    private bool _canUndo;

    // Temporary undo/redo solution. Refactored to support delete tool.
    private readonly Stack<IDrawable> _drawStack = new();
    private readonly Stack<IDrawable> _redoStack = new();
    private readonly CanvasProperties _canvasProperties = new();

    public DrawingCanvas(IDrawingTool drawingTool)
    {
        DrawingTool = drawingTool;
    }

    public event EventHandler? RequestRedraw;
    public event EventHandler<bool>? CanClearChanged;
    public event EventHandler<bool>? CanRedoChanged;
    public event EventHandler<bool>? CanUndoChanged;

    public IDrawingTool DrawingTool { get; set; }


    public void Draw(ICanvas canvas, RectF dirtyRect)
    {
        foreach (IDrawable drawable in _drawStack)
        {
            drawable.Draw(canvas, dirtyRect);
        }
    }

    public void DoDrawingEvent(PointF point)
    {
        if (!_isDrawing)
        {
            _redoStack.Clear();
            _isDrawing = true;
            _drawStack.Push(DrawingTool.BeginDraw(_canvasProperties, point));
            UpdateButtons();
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

    public void Undo()
    {
        if (_drawStack.Count == 0)
            return;

        _redoStack.Push(_drawStack.Pop());
        UpdateButtons();
        Redraw();
    }

    public void Redo()
    {
        if (_redoStack.Count == 0)
            return;

        _drawStack.Push(_redoStack.Pop());
        UpdateButtons();
        Redraw();
    }

    public void Clear()
    {
        _drawStack.Clear();
        _redoStack.Clear();
        UpdateButtons();
        Redraw();
    }

    private void Redraw()
    {
        RequestRedraw?.Invoke(this, EventArgs.Empty);
    }

    private void UpdateButtons()
    {
        bool currentCanUndo = _drawStack.Count > 0;
        bool currentCanRedo = _redoStack.Count > 0;


        if (currentCanUndo != _canUndo)
        {
            _canUndo = currentCanUndo;
            CanUndoChanged?.Invoke(this, _canUndo);
            CanClearChanged?.Invoke(this, _canUndo);
        }

        if (currentCanRedo != _canRedo)
        {
            _canRedo = currentCanRedo;
            CanRedoChanged?.Invoke(this, _canRedo);
        }
    }
}