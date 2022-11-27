using SketchOverlay.Library.Drawing;

namespace SketchOverlay.Drawing;

// TODO: Tests.
public class DrawingCanvas : IDrawingCanvas<IDrawable>, IDrawable
{
    private bool _isDrawing;
    private bool _canRedo;
    private bool _canUndo;

    // Temporary undo/redo solution. Refactored to support delete tool.
    private readonly Stack<IDrawable> _drawStack = new();
    private readonly Stack<IDrawable> _redoStack = new();

    public DrawingCanvas(IDrawingTool<IDrawable> drawingTool)
    {
        DrawingTool = drawingTool;
    }

    public event EventHandler? RequestRedraw;
    public event EventHandler<bool>? CanClearChanged;
    public event EventHandler<bool>? CanRedoChanged;
    public event EventHandler<bool>? CanUndoChanged;

    public IDrawingTool<IDrawable> DrawingTool { get; set; }

    public void Undo()
    {
        if (_drawStack.Count == 0)
            return;

        _redoStack.Push(_drawStack.Pop());
        UpdateAvailableActions();
        Redraw();
    }

    public void Redo()
    {
        if (_redoStack.Count == 0)
            return;

        _drawStack.Push(_redoStack.Pop());
        UpdateAvailableActions();
        Redraw();
    }

    public void Clear()
    {
        _drawStack.Clear();
        _redoStack.Clear();
        UpdateAvailableActions();
        Redraw();
    }

    public void DoDrawingEvent(System.Drawing.PointF point)
    {
        if (!_isDrawing)
        {
            _isDrawing = true;
            _redoStack.Clear();
            _drawStack.Push(DrawingTool.BeginDraw(point));
        }
        else
        {
            DrawingTool.ContinueDraw(point);
        }

        Redraw();
    }

    public void FinalizeDrawingEvent()
    {
        if (!_isDrawing)
            return;

        _isDrawing = false;
        DrawingTool.EndDraw();
        UpdateAvailableActions();
        Redraw();
    }

    public void CancelDrawingEvent()
    {
        if (!_isDrawing)
            return;

        _isDrawing = false;
        DrawingTool.EndDraw();
        _drawStack.Pop();
        UpdateAvailableActions();
        Redraw();
    }

    public void Draw(ICanvas canvas, RectF dirtyRect)
    {
        foreach (IDrawable drawable in _drawStack.Reverse())
        {
            drawable.Draw(canvas, dirtyRect);
        }
    }

    private void Redraw()
    {
        RequestRedraw?.Invoke(this, EventArgs.Empty);
    }

    private void UpdateAvailableActions()
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