using CommunityToolkit.Mvvm.Messaging.Messages;
using SketchOverlay.Messages.Actions;

namespace SketchOverlay.Messages;

public class RequestCanvasActionMessage : ValueChangedMessage<CanvasAction>
{
    public RequestCanvasActionMessage(CanvasAction value) : base(value)
    {
    }
}