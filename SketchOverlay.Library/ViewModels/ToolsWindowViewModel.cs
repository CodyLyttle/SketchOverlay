using System.Drawing;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using SketchOverlay.Library.Drawing;
using SketchOverlay.Library.Drawing.Canvas;
using SketchOverlay.Library.Drawing.Tools;
using SketchOverlay.Library.Messages;
using SketchOverlay.Library.Models;

namespace SketchOverlay.Library.ViewModels;

// TODO: Tests.
public partial class ToolsWindowViewModel<TDrawing, TImageSource, TColor> : ObservableObject,
    IRecipient<ToolsWindowSetPropertyMessage>,
    IRecipient<ToolsWindowDragEventMessage>
{
    private readonly IMessenger _messenger;
    private bool _isDragInProgress;
    private bool _isVisible;
    private readonly ICanvasProperties<TColor> _canvasProps;
    private readonly ICanvasStateManager _canvasStateManager;

    public ToolsWindowViewModel(
        ICanvasProperties<TColor> canvasProps,
        IColorPalette<TColor> drawingColors,
        IDrawingToolCollection<TDrawing, TImageSource, TColor> drawingTools,
        ICanvasStateManager canvasStateManager,
        IMessenger messenger)
    {
        _canvasStateManager = canvasStateManager;
        _canvasStateManager.CanClearChanged += (_, val) => CanClear = val;
        _canvasStateManager.CanRedoChanged += (_, val) => CanRedo= val;
        _canvasStateManager.CanUndoChanged += (_, val) => CanUndo = val;

        _messenger = messenger;
        _messenger.Register<ToolsWindowSetPropertyMessage>(this);
        _messenger.Register<ToolsWindowDragEventMessage>(this);

        _canvasProps = canvasProps;
        _drawingColors = drawingColors;
        DrawingTools = drawingTools;
        SelectedToolInfo = DrawingTools.SelectedToolInfo;
        IsVisible = false;
    }

    [ObservableProperty]
    private IColorPalette<TColor> _drawingColors;

    [ObservableProperty]
    private bool _isInputTransparent;

    [ObservableProperty]
    private double _windowHeight;

    [ObservableProperty]
    private double _windowWidth;

    [ObservableProperty]
    private LibraryThickness _windowMargin;

    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(ClearCommand))]
    private bool _canClear;

    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(RedoCommand))]
    private bool _canRedo;

    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(UndoCommand))]
    private bool _canUndo;

    [RelayCommand(CanExecute = nameof(CanClear))]
    private void Clear() => _canvasStateManager.Clear();

    [RelayCommand(CanExecute = nameof(CanRedo))]
    private void Redo() => _canvasStateManager.Redo();

    [RelayCommand(CanExecute = nameof(CanUndo))]
    private void Undo() => _canvasStateManager.Undo();

    public float MinimumStrokeSize => _canvasProps.MinimumStrokeSize;

    public float MaximumStrokeSize => _canvasProps.MaximumStrokeSize;

    public float StrokeSize
    {
        get => _canvasProps.StrokeSize;
        set
        {
            if (value < MinimumStrokeSize ||
                value > MaximumStrokeSize)
                return;

            _canvasProps.StrokeSize = (float)Math.Round(value);
            OnPropertyChanged();
        }
    }

    public TColor StrokeColor
    {
        get => _canvasProps.StrokeColor;
        set
        {
            if (value is null)
                return;

            _canvasProps.StrokeColor = value;
        }
    }

    public TColor FillColor
    {
        get => _canvasProps.FillColor;
        set
        {
            if (value is null)
                return;

            _canvasProps.FillColor = value;
        }
    }

    // DrawingToolsCollection.SelectedItem was originally bound to DrawingTools.SelectedToolInfo,
    // however, when the control is loaded, the SelectedItem value gets set to null.
    // This causes a NullReferenceException when attempting to draw without explicitly selecting a tool.
    // We workaround this issue by preventing the view from setting the value to null.
    // Bug: https://github.com/dotnet/maui/issues/8572
    public DrawingToolInfo<TDrawing, TImageSource, TColor> SelectedToolInfo
    {
        get => DrawingTools.SelectedToolInfo;
        set
        {
            // ReSharper disable once ConditionIsAlwaysTrueOrFalseAccordingToNullableAPIContract
            if (value is null || value == DrawingTools.SelectedToolInfo)
                return;

            DrawingTools.SelectedToolInfo = value;
            OnPropertyChanged();
        }
    }

    public IDrawingToolCollection<TDrawing, TImageSource, TColor> DrawingTools { get; }

    public bool IsDragInProgress
    {
        get => _isDragInProgress;
        private set
        {
            if (value == _isDragInProgress)
                return;

            _isDragInProgress = value;
            OnPropertyChanged();
            _messenger.Send(new ToolsWindowPropertyChangedMessage(value));
        }
    }

    public bool IsVisible
    {
        get => _isVisible;
        private set
        {
            if (value == _isVisible)
                return;

            _isVisible = value;
            OnPropertyChanged();
            _messenger.Send(new ToolsWindowPropertyChangedMessage(value));
        }
    }

    public void Receive(ToolsWindowSetPropertyMessage message)
    {
        switch (message.PropertyName)
        {
            case nameof(IsVisible):
                IsVisible = (bool)message.Value;
                break;
            case nameof(IsInputTransparent):
                IsInputTransparent = (bool)message.Value;
                break;
            case nameof(CanClear):
                CanClear = (bool)message.Value;
                break;
            case nameof(CanRedo):
                CanRedo = (bool)message.Value;
                break;
            case nameof(CanUndo):
                CanUndo = (bool)message.Value;
                break;
        }
    }

    public void Receive(ToolsWindowDragEventMessage message)
    {
        (DragAction action, PointF position) = message.Value;

        switch (action)
        {
            case DragAction.BeginDrag:
                if (IsDragInProgress)
                {
                    throw new InvalidOperationException("The window is already being dragged");
                }
                IsDragInProgress = true;
                IsInputTransparent = true;
                break;
            case DragAction.ContinueDrag:
                if (!IsDragInProgress)
                {
                    throw new InvalidOperationException("The window is not being dragged");
                }
                break;
            case DragAction.EndDrag:
                if (!IsDragInProgress)
                {
                    throw new InvalidOperationException("The window is not being dragged");
                }
                IsDragInProgress = false;
                IsInputTransparent = false;
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(message), "Invalid drag action");
        }

        double left = position.X - WindowWidth / 2;
        double top = position.Y;
        WindowMargin = new LibraryThickness(left, top);
    }
}