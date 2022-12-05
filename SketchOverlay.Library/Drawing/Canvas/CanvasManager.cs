using System.Drawing;
using SketchOverlay.Library.Drawing.Tools;

namespace SketchOverlay.Library.Drawing.Canvas;

public class CanvasManager<TDrawing, TOutput, TColor> : ICanvasManager<TOutput>
{
    private bool _canClear;
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

    public bool CanClear
    {
        get => _canClear;
        private set
        {
            if (value == _canClear) return;
            _canClear = value;
            CanClearChanged?.Invoke(this, value);
        }
    }

    public bool CanRedo
    {
        get => _canRedo;
        private set
        {
            if (value == _canRedo) return;
            _canRedo = value;
            CanRedoChanged?.Invoke(this, value);
        }
    }

    public bool CanUndo
    {
        get => _canUndo;
        private set
        {
            if (value == _canUndo) return;
            _canUndo = value;
            CanUndoChanged?.Invoke(this, value);
        }
    }

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
        if (!CanUndo) return;

        _redoStack.Push(_drawStack.PopDrawing());
        Update();
    }

    public void Redo()
    {
        if (!CanRedo) return;

        _drawStack.PushDrawing(_redoStack.Pop());
        Update();
    }

    public void Clear()
    {
        if (!CanClear) return;

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
        CanUndo = _drawStack.Count > 0;
        CanClear = CanUndo;
        CanRedo = _redoStack.Count > 0;
    }
}