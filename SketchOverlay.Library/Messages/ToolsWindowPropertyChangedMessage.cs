using System.Runtime.CompilerServices;

namespace SketchOverlay.Library.Messages;

public class ToolsWindowPropertyChangedMessage : SimplePropertyChangedMessage
{
    public ToolsWindowPropertyChangedMessage(object value, [CallerMemberName]string? propertyName = null) 
        : base(value, propertyName)
    {
    }
}