using CommunityToolkit.Mvvm.Messaging.Messages;
using SketchOverlay.Messages.Actions;

namespace SketchOverlay.Messages;

public class OverlayWindowCanvasActionMessage : ValueChangedMessage<CanvasAction>
{
    public OverlayWindowCanvasActionMessage(CanvasAction value) : base(value)
    {
    }
}