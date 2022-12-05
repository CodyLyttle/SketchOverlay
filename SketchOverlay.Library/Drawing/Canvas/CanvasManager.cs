using System.Drawing;
using SketchOverlay.Library.Drawing.Tools;

namespace SketchOverlay.Library.Drawing.Canvas;

public class CanvasManager<TDrawing, TOutput, TColor> : ICanvasManager<TOutput>
{
    // Temporary undo/redo solution. Refactored to support delete tool.
    private readonly Stack<TDrawing> _redoStack = new();
    private readonly IDrawingStack<TDrawing, TOutput> _drawStack;
    private readonly IDrawingToolRetriever<TDrawing, TColor> _toolRetriever;
    private readonly ICanvasProperties<TColor> _canvasProperties;

    public CanvasManager(ICanvasProperties<TColor> canvasProperties,
        IDrawingStack<TDrawing, TOutput> drawStack,
        IDrawingToolRetriever<TDrawing, TColor> toolRetriever)
    {
        _drawStack = drawStack;
        _canvasProperties = canvasProperties;
        _toolRetriever = toolRetriever;
    }

    public event EventHandler? RequestRedraw;
    public event EventHandler<bool>? CanClearChanged;
    public event EventHandler<bool>? CanRedoChanged;
    public event EventHandler<bool>? CanUndoChanged;

    private IDrawingTool<TDrawing, TColor> DrawingTool => _toolRetriever.SelectedTool;

    public bool CanUndo { get; private set; }
    public bool CanRedo { get; private set; }
    public bool CanClear => CanUndo;
    public bool IsDrawing { get; private set; }
    public TOutput DrawingOutput => _drawStack.Output;

    public void DoDrawing(PointF point)
    {
        if (!IsDrawing)
        {
            IsDrawing = true;
            _redoStack.Clear();
            TDrawing drawing = DrawingTool.CreateDrawing(_canvasProperties, point);
            _drawStack.PushDrawing(drawing);
        }
        else
        {
            DrawingTool.UpdateDrawing(point);
        }

        Update(updateAvailableActions: false);
    }

    public void FinishDrawing()
    {
        if (!IsDrawing) return;

        IsDrawing = false;
        DrawingTool.FinishDrawing();
        Update();
    }

    public void CancelDrawing()
    {
        if (!IsDrawing) return;

        IsDrawing = false;
        DrawingTool.FinishDrawing();
        _drawStack.PopDrawing();
        Update(updateAvailableActions: false);
    }

    public void Undo()
    {
        if (_drawStack.Count is 0) return;

        _redoStack.Push(_drawStack.PopDrawing());
        Update();
    }

    public void Redo()
    {
        if (_redoStack.Count is 0) return;

        _drawStack.PushDrawing(_redoStack.Pop());
        Update();
    }

    public void Clear()
    {
        _drawStack.Clear();
        _redoStack.Clear();
        Update();
    }

    private void Update(bool updateAvailableActions = true)
    {
        if (updateAvailableActions)
            UpdateAvailableActions();

        DoUpdate();
    }

    protected virtual void DoUpdate()
    {
        RequestRedraw?.Invoke(this, EventArgs.Empty);
    }

    private void UpdateAvailableActions()
    {
        bool updatedCanUndo = _drawStack.Count > 0;
        bool updatedCanRedo = _redoStack.Count > 0;

        if (updatedCanUndo != CanUndo)
        {
            CanUndo = updatedCanUndo;
            CanUndoChanged?.Invoke(this, CanUndo);
            CanClearChanged?.Invoke(this, CanUndo);
        }

        if (updatedCanRedo != CanRedo)
        {
            CanRedo = updatedCanRedo;
            CanRedoChanged?.Invoke(this, CanRedo);
        }
    }
}