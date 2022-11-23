using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using SketchOverlay.Drawing;
using SketchOverlay.Messages;
using SketchOverlay.Messages.Actions;
using SketchOverlay.Models;

namespace SketchOverlay.ViewModels;

public partial class DrawingToolWindowViewModel : ObservableObject
{
    private Color? _selectedDrawingColor;
    private DrawingToolInfo? _selectedDrawingTool;
    private double _selectedDrawingSize;
    private readonly IMessenger _messenger;

    public DrawingToolWindowViewModel(IMessenger messenger)
    {
        _messenger = messenger;
        _selectedDrawingColor = GlobalDrawingValues.DefaultDrawingColor;
        _selectedDrawingTool = GlobalDrawingValues.DefaultDrawingTool;
        // BUG: CollectionView.SelectedItem reverts to null after this point. See - https://github.com/dotnet/maui/issues/8572
        // Potential workaround: Set initial values when tool window is first displayed.

        SelectedDrawingSize = GlobalDrawingValues.DefaultDrawingSize;
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
        _messenger.Send(new CanvasActionMessage(CanvasAction.Undo));

    [RelayCommand]
    private void Redo() =>
        _messenger.Send(new CanvasActionMessage(CanvasAction.Redo));

    [RelayCommand]
    private void Clear() =>
        _messenger.Send(new CanvasActionMessage(CanvasAction.Clear));
}