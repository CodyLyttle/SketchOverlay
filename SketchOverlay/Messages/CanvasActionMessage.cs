using CommunityToolkit.Mvvm.Messaging.Messages;
using SketchOverlay.Drawing.Canvas;

namespace SketchOverlay.Messages;
internal class CanvasActionMessage : ValueChangedMessage<CanvasAction>
{
    public CanvasActionMessage(CanvasAction value) : base(value)
    {
    }
}