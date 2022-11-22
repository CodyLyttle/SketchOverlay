using CommunityToolkit.Mvvm.Messaging.Messages;

namespace SketchOverlay.Messages;

public class DrawingColorChangedMessage : ValueChangedMessage<Color>
{
    public DrawingColorChangedMessage(Color value) : base(value)
    {
    }
}