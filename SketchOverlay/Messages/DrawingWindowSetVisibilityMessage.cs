using CommunityToolkit.Mvvm.Messaging.Messages;

namespace SketchOverlay.Messages;

public class DrawingWindowSetVisibilityMessage : ValueChangedMessage<bool>
{
    public DrawingWindowSetVisibilityMessage(bool value) : base(value)
    {
    }
}