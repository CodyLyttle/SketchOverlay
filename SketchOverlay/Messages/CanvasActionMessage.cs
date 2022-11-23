using CommunityToolkit.Mvvm.Messaging.Messages;
using SketchOverlay.Messages.Actions;

namespace SketchOverlay.Messages;
public class CanvasActionMessage : ValueChangedMessage<CanvasAction>
{
    public CanvasActionMessage(CanvasAction value) : base(value)
    {
    }
}