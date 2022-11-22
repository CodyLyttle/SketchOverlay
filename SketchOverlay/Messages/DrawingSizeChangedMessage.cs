using CommunityToolkit.Mvvm.Messaging.Messages;

namespace SketchOverlay.Messages;
public class DrawingSizeChangedMessage : ValueChangedMessage<float>
{
    public DrawingSizeChangedMessage(float value) : base(value)
    {
    }
}