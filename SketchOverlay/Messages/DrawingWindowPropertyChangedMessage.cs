using System.Runtime.CompilerServices;

namespace SketchOverlay.Messages;

public class DrawingWindowPropertyChangedMessage : SimplePropertyChangedMessage
{
    public DrawingWindowPropertyChangedMessage(object value, [CallerMemberName]string? propertyName = null) 
        : base(value, propertyName)
    {
    }
}