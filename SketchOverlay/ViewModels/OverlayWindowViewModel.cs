using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using SketchOverlay.Drawing.Canvas;
using SketchOverlay.Messages;

namespace SketchOverlay.ViewModels;

public partial class OverlayWindowViewModel : ObservableObject, 
    IRecipient<CanvasActionMessage>,
    IRecipient<DrawingColorChangedMessage>,
    IRecipient<DrawingToolChangedMessage>,
    IRecipient<DrawingSizeChangedMessage>
{
    public IDrawingCanvas Canvas { get; }

    public OverlayWindowViewModel(IDrawingCanvas canvas, IMessenger messenger)
    {
        Canvas = canvas;
        messenger.Register<CanvasActionMessage>(this);
        messenger.Register<DrawingColorChangedMessage>(this);
        messenger.Register<DrawingToolChangedMessage>(this);
        messenger.Register<DrawingSizeChangedMessage>(this);
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

    public void Receive(DrawingToolChangedMessage message)
    {
        Canvas.DrawingTool = message.Value;
    }

    public void Receive(DrawingSizeChangedMessage message)
    {
        Canvas.StrokeSize = message.Value;
    }
}
