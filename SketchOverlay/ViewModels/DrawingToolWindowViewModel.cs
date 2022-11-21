using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using SketchOverlay.Drawing.Canvas;
using SketchOverlay.Messages;

namespace SketchOverlay.ViewModels;

public partial class DrawingToolWindowViewModel : ObservableObject
{
    public DrawingToolWindowViewModel()
    {
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