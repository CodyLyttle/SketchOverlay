using CommunityToolkit.Mvvm.Messaging.Messages;

namespace SketchOverlay.Messages;


// TODO: Look at using RequestMessage instead.
// See: https://github.com/CommunityToolkit/dotnet/tree/main/src/CommunityToolkit.Mvvm/Messaging/Messages
public class DrawingWindowVisibilityChangedMessage : ValueChangedMessage<bool>
{
    public DrawingWindowVisibilityChangedMessage(bool value) : base(value)
    {
    }
}