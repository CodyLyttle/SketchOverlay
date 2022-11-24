using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using SketchOverlay.Drawing;
using SketchOverlay.Messages;
using SketchOverlay.Messages.Actions;
using SketchOverlay.Models;
using Thickness = Microsoft.Maui.Thickness;

namespace SketchOverlay.ViewModels;

public partial class DrawingToolWindowViewModel : ObservableObject,
    IRecipient<DrawingWindowSetPropertyMessage>,
    IRecipient<DrawingWindowDragEventMessage>
{
    private Color? _selectedDrawingColor;
    private DrawingToolInfo? _selectedDrawingTool;
    private double _selectedDrawingSize;
    private readonly IMessenger _messenger;
    private bool _isDragInProgress;
    private bool _isVisible;

    public DrawingToolWindowViewModel(IMessenger messenger)
    {
        _messenger = messenger;
        _messenger.Register<DrawingWindowSetPropertyMessage>(this);
        _messenger.Register<DrawingWindowDragEventMessage>(this);

        _selectedDrawingColor = GlobalDrawingValues.DefaultDrawingColor;
        _selectedDrawingTool = GlobalDrawingValues.DefaultDrawingTool;
        OnPropertyChanged(nameof(SelectedDrawingColor));
        OnPropertyChanged(nameof(SelectedDrawingTool));
        // BUG: CollectionView.SelectedItem reverts to null after this point. See - https://github.com/dotnet/maui/issues/8572
        // Potential workaround: Set initial values when tool window is first displayed.

        SelectedDrawingSize = GlobalDrawingValues.DefaultDrawingSize;
        IsVisible = false;

    }

    [ObservableProperty]
    private bool _isInputTransparent;

    [ObservableProperty]
    private double _windowHeight = 300;

    [ObservableProperty]
    private double _windowWidth = 300;

    [ObservableProperty]
    private Thickness _windowMargin;

    [ObservableProperty] 
    private bool _canClear;

    [ObservableProperty]
    private bool _canRedo;

    [ObservableProperty]
    private bool _canUndo;

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

    public Color? SelectedDrawingColor
    {
        get => _selectedDrawingColor;
        set
        {
            if (EqualityComparer<Color>.Default.Equals(value, _selectedDrawingColor))
                return;

            // Sending a message while value is null throws a NullReferenceException.
            if (value == null)
                return;

            _selectedDrawingColor = value;
            OnPropertyChanged();
            _messenger.Send(new DrawingWindowPropertyChangedMessage(value));
        }
    }

    public DrawingToolInfo? SelectedDrawingTool
    {
        get => _selectedDrawingTool;
        set
        {
            if (value == _selectedDrawingTool)
                return;

            // Sending a message while value is null throws a NullReferenceException.
            if (value == null)
                return;

            _selectedDrawingTool = value;
            OnPropertyChanged();
            _messenger.Send(new DrawingWindowPropertyChangedMessage(value));
        }
    }

    public double SelectedDrawingSize
    {
        get => _selectedDrawingSize;
        set
        {
            value = value switch
            {
                < GlobalDrawingValues.MinimumDrawingSize => GlobalDrawingValues.MinimumDrawingSize,
                > GlobalDrawingValues.MaximumDrawingSize => GlobalDrawingValues.MaximumDrawingSize,
                _ => value
            };

            if (Math.Abs(value - _selectedDrawingSize) < 0.1)
                return;

            _selectedDrawingSize = value;
            OnPropertyChanged();
            _messenger.Send(new DrawingWindowPropertyChangedMessage(value));
        }
    }

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
        WindowMargin = new Thickness(left, top, 0, 0);
    }
}