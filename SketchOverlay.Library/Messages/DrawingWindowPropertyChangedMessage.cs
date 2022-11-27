using System.Runtime.CompilerServices;

namespace SketchOverlay.Library.Messages;

public class DrawingWindowPropertyChangedMessage : SimplePropertyChangedMessage
{
    public DrawingWindowPropertyChangedMessage(object value, [CallerMemberName]string? propertyName = null) 
        : base(value, propertyName)
    {
    }
}