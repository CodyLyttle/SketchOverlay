namespace SketchOverlay.Messages;

public class SimpleSetPropertyMessage
{
    public SimpleSetPropertyMessage(object value, string propertyName)
    {
        PropertyName = propertyName;
        Value = value;
    }

    public string PropertyName { get; }
    public object Value { get; }
}