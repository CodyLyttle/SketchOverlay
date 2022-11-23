using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using SketchOverlay.Drawing;
using SketchOverlay.Messages;
using SketchOverlay.Messages.Actions;
using SketchOverlay.Models;

namespace SketchOverlay.ViewModels;

public partial class DrawingToolWindowViewModel : ObservableObject,
    IRecipient<DrawingWindowSetVisibilityMessage>,
    IRecipient<DrawingWindowDragEventMessage>
{
    private Color? _selectedDrawingColor;
    private DrawingToolInfo? _selectedDrawingTool;
    private double _selectedDrawingSize;
    private readonly IMessenger _messenger;
    private bool _isVisible;
    private bool _isDragging;

    public DrawingToolWindowViewModel(IMessenger messenger)
    {
        _messenger = messenger;
        _selectedDrawingColor = GlobalDrawingValues.DefaultDrawingColor;
        _selectedDrawingTool = GlobalDrawingValues.DefaultDrawingTool;
        // BUG: CollectionView.SelectedItem reverts to null after this point. See - https://github.com/dotnet/maui/issues/8572
        // Potential workaround: Set initial values when tool window is first displayed.

        SelectedDrawingSize = GlobalDrawingValues.DefaultDrawingSize;
        IsVisible = false;

        _messenger.Register<DrawingWindowSetVisibilityMessage>(this);
        _messenger.Register<DrawingWindowDragEventMessage>(this);
    }

    [ObservableProperty]
    private double _windowHeight = 300;

    [ObservableProperty]
    private double _windowWidth = 300;

    [ObservableProperty]
    private Thickness _windowMargin;

    [ObservableProperty]
    private bool _isInputTransparent;

    public bool IsVisible
    {
        get => _isVisible;
        set
        {
            if (value == _isVisible)
                return;

            _isVisible = value;
            OnPropertyChanged();

            _messenger.Send(new DrawingWindowVisibilityChangedMessage(value));
        }
    }

    public Color? SelectedDrawingColor
    {
        get => _selectedDrawingColor;
        set
        {
            if (EqualityComparer<Color>.Default.Equals(value, _selectedDrawingColor))
                return;

            _selectedDrawingColor = value;

            // Sending a message while value is null throws a NullReferenceException.
            if (value == null)
                return;

            _messenger.Send(new DrawingColorChangedMessage(value));
            OnPropertyChanged(nameof(SelectedDrawingColor));
        }
    }

    public DrawingToolInfo? SelectedDrawingTool
    {
        get => _selectedDrawingTool;
        set
        {
            if (value == _selectedDrawingTool)
                return;

            _selectedDrawingTool = value;

            // Sending a message while value is null throws a NullReferenceException.
            if (value == null)
                return;

            _messenger.Send(new DrawingToolChangedMessage(value.Tool));
        }
    }

    public double SelectedDrawingSize
    {
        get => _selectedDrawingSize;
        set
        {
            if (Math.Abs(value - _selectedDrawingSize) < 0.1)
                return;

            if (value < GlobalDrawingValues.MinimumDrawingSize)
                _selectedDrawingSize = GlobalDrawingValues.MinimumDrawingSize;
            else if (value > GlobalDrawingValues.MaximumDrawingSize)
                _selectedDrawingSize = GlobalDrawingValues.MaximumDrawingSize;
            else
                _selectedDrawingSize = value;

            _messenger.Send(new DrawingSizeChangedMessage((float)_selectedDrawingSize));
        }
    }

    [RelayCommand]
    private void Undo() =>
        _messenger.Send(new RequestCanvasActionMessage(CanvasAction.Undo));

    [RelayCommand]
    private void Redo() =>
        _messenger.Send(new RequestCanvasActionMessage(CanvasAction.Redo));

    [RelayCommand]
    private void Clear() =>
        _messenger.Send(new RequestCanvasActionMessage(CanvasAction.Clear));

    public void Receive(DrawingWindowSetVisibilityMessage message) =>
        IsVisible = message.Value;

    public void Receive(DrawingWindowDragEventMessage message)
    {
        (DragAction action, PointF position) = message.Value;

        switch (action)
        {
            case DragAction.BeginDrag:
                _isDragging = true;
                IsInputTransparent = true;
                break;
            case DragAction.ContinueDrag:
                if (!_isDragging)
                    throw new InvalidOperationException(
                        $"{nameof(DragAction.ContinueDrag)} was requested before {nameof(DragAction.ContinueDrag)}");
                break;
            case DragAction.EndDrag:
                IsInputTransparent = false;
                _isDragging = false;
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(message), "Invalid drag action");
        }

        double left = position.X - WindowWidth / 2;
        double top = position.Y;
        WindowMargin = new Thickness(left, top, 0, 0);
    }
}