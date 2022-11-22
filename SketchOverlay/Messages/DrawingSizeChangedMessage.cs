using CommunityToolkit.Mvvm.Messaging.Messages;

namespace SketchOverlay.Messages;
internal class DrawingSizeChangedMessage : ValueChangedMessage<double>
{
    public DrawingSizeChangedMessage(double value) : base(value)
    {
    }
}