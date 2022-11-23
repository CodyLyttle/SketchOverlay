using CommunityToolkit.Mvvm.Messaging.Messages;
using SketchOverlay.Models;

namespace SketchOverlay.Messages;

public class OverlayWindowDrawActionMessage : ValueChangedMessage<DrawActionInfo>
{
    public OverlayWindowDrawActionMessage(DrawActionInfo value) : base(value)
    {
    }
}