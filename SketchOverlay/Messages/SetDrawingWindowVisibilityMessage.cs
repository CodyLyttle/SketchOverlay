using CommunityToolkit.Mvvm.Messaging.Messages;

namespace SketchOverlay.Messages;

public class SetDrawingWindowVisibilityMessage : ValueChangedMessage<bool>
{
    public SetDrawingWindowVisibilityMessage(bool value) : base(value)
    {
    }
}