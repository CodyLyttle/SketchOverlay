using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using SketchOverlay.Drawing.Canvas;
using SketchOverlay.Messages;

namespace SketchOverlay.ViewModels;

public partial class OverlayWindowViewModel : ObservableObject, 
    IRecipient<CanvasActionMessage>,
    IRecipient<DrawingColorChangedMessage>
{
    public IDrawingCanvas Canvas { get; }

    public OverlayWindowViewModel(IDrawingCanvas canvas)
    {
        Canvas = canvas;
        WeakReferenceMessenger.Default.Register<CanvasActionMessage>(this);
        WeakReferenceMessenger.Default.Register<DrawingColorChangedMessage>(this);
    }

    public void Receive(CanvasActionMessage message)
    {
        switch (message.Value)
        {
            case CanvasAction.Undo:
                Canvas.Undo();
                break;
            case CanvasAction.Redo:
                Canvas.Redo();
                break;
            case CanvasAction.Clear:
                Canvas.Clear();
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(message));
        }
    }

    public void Receive(DrawingColorChangedMessage message)
    {
        Canvas.StrokeColor = message.Value;
    }
}
