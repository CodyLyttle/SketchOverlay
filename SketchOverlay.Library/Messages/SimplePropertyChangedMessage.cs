using System.Runtime.CompilerServices;

namespace SketchOverlay.Library.Messages;

public class SimplePropertyChangedMessage
{
    public SimplePropertyChangedMessage(object value, [CallerMemberName] string? propertyName = null)
    {
        ArgumentNullException.ThrowIfNull(propertyName);
        PropertyName = propertyName;
        Value = value;
    }

    public string PropertyName { get; set; }
    public object Value { get; set; }
}