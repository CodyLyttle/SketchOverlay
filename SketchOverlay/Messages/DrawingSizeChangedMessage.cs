using CommunityToolkit.Mvvm.Messaging.Messages;

namespace SketchOverlay.Messages;

// TODO: Look at using PropertyChangedMessage instead.
// See: https://github.com/CommunityToolkit/dotnet/tree/main/src/CommunityToolkit.Mvvm/Messaging/Messages
public class DrawingSizeChangedMessage : ValueChangedMessage<float>
{
    public DrawingSizeChangedMessage(float value) : base(value)
    {
    }
}