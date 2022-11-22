using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using SketchOverlay.Drawing.Canvas;
using SketchOverlay.Drawing.Tools;
using SketchOverlay.Messages;
using SketchOverlay.Models;

namespace SketchOverlay.ViewModels;

public partial class DrawingToolWindowViewModel : ObservableObject
{
    private Color? _selectedDrawingColor;
    private DrawingToolInfo? _selectedDrawingTool;
    private double _selectedDrawingSize;

    public DrawingToolWindowViewModel()
    {
        SelectedDrawingSize = MinimumDrawingSize;
        _selectedDrawingColor = DrawingColors[0];
        _selectedDrawingTool = DrawingTools[0];
        // BUG: CollectionView.SelectedItem reverts to null after this point. See - https://github.com/dotnet/maui/issues/8572
        // Potential workaround: Set initial values when tool window is first displayed.
    }

    public Color[] DrawingColors { get; } =
    {
        Colors.Red, Colors.Green, Colors.Blue,
        Colors.Magenta, Colors.Yellow
    };

    public DrawingToolInfo[] DrawingTools { get; } =
    {
        new(new BrushTool(), ImageSource.FromFile("placeholder_paintbrush.png"), "Paintbrush"),
        new(new LineTool(), ImageSource.FromFile("placeholder_line.png"), "Line"),
        new(new RectangleTool(), ImageSource.FromFile("placeholder_rectangle.png"), "Rectangle")
    };

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

            WeakReferenceMessenger.Default.Send(new DrawingColorChangedMessage(value));
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

            WeakReferenceMessenger.Default.Send(new DrawingToolChangedMessage(value.Tool));
        }
    }

    public double MinimumDrawingSize { get; } = 1;

    public double MaximumDrawingSize { get; } = 32;

    public double SelectedDrawingSize
    {
        get => _selectedDrawingSize;
        set
        {
            if (Math.Abs(value - _selectedDrawingSize) < 0.1)
                return;

            if (value < MinimumDrawingSize)
                _selectedDrawingSize = MinimumDrawingSize;
            else if (value > MaximumDrawingSize)
                _selectedDrawingSize = MaximumDrawingSize;
            else
                _selectedDrawingSize = value;

            WeakReferenceMessenger.Default.Send(new DrawingSizeChangedMessage(_selectedDrawingSize));
        }
    }

    [RelayCommand]
    private static void Undo() =>
        WeakReferenceMessenger.Default.Send(new CanvasActionMessage(CanvasAction.Undo));

    [RelayCommand]
    private static void Redo() =>
        WeakReferenceMessenger.Default.Send(new CanvasActionMessage(CanvasAction.Redo));

    [RelayCommand]
    private static void Clear() =>
        WeakReferenceMessenger.Default.Send(new CanvasActionMessage(CanvasAction.Clear));
}