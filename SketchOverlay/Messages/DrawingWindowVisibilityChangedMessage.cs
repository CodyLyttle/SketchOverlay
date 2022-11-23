using CommunityToolkit.Mvvm.Messaging.Messages;

namespace SketchOverlay.Messages;

public class DrawingWindowVisibilityChangedMessage : ValueChangedMessage<bool>
{
    public DrawingWindowVisibilityChangedMessage(bool value) : base(value)
    {
    }
}