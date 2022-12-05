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

public partial class DrawingToolWindowViewModel<TDrawing, TImageSource, TColor> : ObservableObject,
    IRecipient<DrawingWindowSetPropertyMessage>,
    IRecipient<DrawingWindowDragEventMessage>
{
    private readonly IMessenger _messenger;
    private bool _isDragInProgress;
    private bool _isVisible;
    private readonly ICanvasProperties<TColor> _canvasProps;

    public DrawingToolWindowViewModel(
        ICanvasProperties<TColor> canvasProps, 
        IColorPalette<TColor> drawingColors,
        IDrawingToolCollection<TDrawing, TImageSource, TColor> drawingTools, 
        IMessenger messenger)
    {
        _messenger = messenger;
        _messenger.Register<DrawingWindowSetPropertyMessage>(this);
        _messenger.Register<DrawingWindowDragEventMessage>(this);

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
    private double _windowHeight = 310;

    [ObservableProperty]
    private double _windowWidth = 300;

    [ObservableProperty]
    private LibraryThickness _windowMargin;

    [ObservableProperty]
    private bool _canClear;

    [ObservableProperty]
    private bool _canRedo;

    [ObservableProperty]
    private bool _canUndo;

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

    public float StrokeSize
    {
        get => _canvasProps.StrokeSize;
        set
        {
            if (value is < ICanvasProperties<TColor>.MinimumStrokeSize
                      or > ICanvasProperties<TColor>.MaximumStrokeSize)
                return;

            _canvasProps.StrokeSize = (float)Math.Round(value);
            OnPropertyChanged();
        }
    }

    public float MinimumStrokeSize => ICanvasProperties<TColor>.MinimumStrokeSize;
    
    public float MaximumStrokeSize => ICanvasProperties<TColor>.MaximumStrokeSize;

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
            _messenger.Send(new DrawingWindowPropertyChangedMessage(value));
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
            _messenger.Send(new DrawingWindowPropertyChangedMessage(value));
        }
    }

    // TODO: Move yield tasks to Maui project.
    // BUG: Button.IsEnabled visual state stops updating after a few click events.
    // See: https://github.com/dotnet/maui/issues/7377
    // Workaround await Task.Yield() before sending message.
    [RelayCommand]
    private async void Undo()
    {
        await Task.Yield();
        _messenger.Send(new OverlayWindowCanvasActionMessage(CanvasAction.Undo));
    }

    [RelayCommand]
    private async void Redo()
    {
        await Task.Yield();
        _messenger.Send(new OverlayWindowCanvasActionMessage(CanvasAction.Redo));
    }

    [RelayCommand]
    private async void Clear()
    {
        await Task.Yield();
        _messenger.Send(new OverlayWindowCanvasActionMessage(CanvasAction.Clear));
    }

    public void Receive(DrawingWindowSetPropertyMessage message)
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

    public void Receive(DrawingWindowDragEventMessage message)
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