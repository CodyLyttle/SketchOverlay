using System.Drawing;
using SketchOverlay.Library.Drawing.Tools;

namespace SketchOverlay.Library.Drawing.Canvas;

public class CanvasManager<TDrawing, TOutput, TColor> : ICanvasManager<TOutput>
{
    private bool _canRedo;
    private bool _canUndo;

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
    
    public TOutput DrawingOutput => _drawStack.Output;
    public bool IsDrawing { get; private set; }

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

    public void DoDrawingEvent(PointF point)
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

    public void FinalizeDrawingEvent()
    {
        if (!IsDrawing) return;

        IsDrawing = false;
        DrawingTool.FinishDrawing();
        Update();
    }

    public void CancelDrawingEvent()
    {
        if (!IsDrawing) return;

        IsDrawing = false;
        DrawingTool.FinishDrawing();
        _drawStack.PopDrawing();
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