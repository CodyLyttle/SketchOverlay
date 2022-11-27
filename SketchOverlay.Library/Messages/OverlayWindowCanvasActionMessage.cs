using CommunityToolkit.Mvvm.Messaging.Messages;
using SketchOverlay.Library.Models;

namespace SketchOverlay.Library.Messages;

public class OverlayWindowCanvasActionMessage : ValueChangedMessage<CanvasAction>
{
    public OverlayWindowCanvasActionMessage(CanvasAction value) : base(value)
    {
    }
}