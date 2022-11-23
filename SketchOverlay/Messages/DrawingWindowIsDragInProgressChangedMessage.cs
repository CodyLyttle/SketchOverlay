using CommunityToolkit.Mvvm.Messaging.Messages;

namespace SketchOverlay.Messages;

public class DrawingWindowIsDragInProgressChangedMessage : ValueChangedMessage<bool>
{
    public DrawingWindowIsDragInProgressChangedMessage(bool value) : base(value)
    {
    }
}
