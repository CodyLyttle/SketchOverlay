using SketchOverlay.Drawing.Tools;

namespace SketchOverlay.Drawing.Canvas;

internal class DrawingCanvas : IDrawable
{
    private bool _isDrawingPrimary;
    private bool _canRedo;
    private bool _canUndo;

    // Temporary undo/redo solution. Refactored to support delete tool.
    private readonly Stack<IDrawable> _drawStack = new();
    private readonly Stack<IDrawable> _redoStack = new();
    private CanvasProperties _canvasProperties = new();

    public DrawingCanvas(IDrawingTool primaryDrawingTool)
    {
        PrimaryDrawingTool = primaryDrawingTool;
    }

    public event EventHandler? RequestRedraw;
    public event EventHandler<bool>? CanClearChanged;
    public event EventHandler<bool>? CanRedoChanged;
    public event EventHandler<bool>? CanUndoChanged;

    public IDrawingTool PrimaryDrawingTool { get; set; }

    public void SetPrimaryStrokeColor(Color color)
    {
        _canvasProperties.StrokeColor = color;
    }

    public void SetPrimaryStrokeSize(float size)
    {
        _canvasProperties.StrokeSize = size;
    }

    public void Draw(ICanvas canvas, RectF dirtyRect)
    {
        foreach (IDrawable drawable in _drawStack.Reverse())
        {
            drawable.Draw(canvas, dirtyRect);
        }
    }

    public void DoPrimaryDrawingEvent(PointF point)
    {
        if (!_isDrawingPrimary)
        {
            _isDrawingPrimary = true;
            _redoStack.Clear();
            _drawStack.Push(PrimaryDrawingTool.BeginDraw(_canvasProperties, point));
            UpdateAvailableActions();
        }
        else
        {
            PrimaryDrawingTool.ContinueDraw(point);
        }

        Redraw();
    }

    public void EndDrawingEvent()
    {
        if (_isDrawingPrimary)
        {
            _isDrawingPrimary = false;
            PrimaryDrawingTool.EndDraw();
        }

        Redraw();
    }

    public void CancelPrimaryDrawingEvent()
    {
        if (_isDrawingPrimary)
        {
            _isDrawingPrimary = false;
            PrimaryDrawingTool.EndDraw();
            _drawStack.Pop();
            UpdateAvailableActions();
            Redraw();
        }

    }

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