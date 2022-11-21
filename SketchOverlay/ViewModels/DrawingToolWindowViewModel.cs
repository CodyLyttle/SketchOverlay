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
    private DrawingToolInfo? _selectedDrawingTool;

    public DrawingToolWindowViewModel()
    {
        SelectedDrawingTool = DrawingTools[0];
    }

    public DrawingToolInfo[] DrawingTools { get; } =
    {
        new(new BrushTool(), ImageSource.FromFile("placeholder_paintbrush.png"), "Paintbrush"),
        new(new LineTool(), ImageSource.FromFile("placeholder_line.png"), "Line"),
        new(new RectangleTool(), ImageSource.FromFile("placeholder_rectangle.png"), "Rectangle")
    };

    public DrawingToolInfo? SelectedDrawingTool
    {
        get => _selectedDrawingTool;
        set
        {
            if (value == _selectedDrawingTool)
                return;

            _selectedDrawingTool = value;
            
            // Sending a message while value is null throws a NullReferenceException.
            // The exception was being thrown during instantiation, even when DrawingToolInfo was non-nullable.
            if (value == null)
                return;

            WeakReferenceMessenger.Default.Send(new DrawingToolChangedMessage(value.Tool));
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