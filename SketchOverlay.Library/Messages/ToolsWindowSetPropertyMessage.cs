namespace SketchOverlay.Library.Messages;

public class ToolsWindowSetPropertyMessage : SimpleSetPropertyMessage
{
    public ToolsWindowSetPropertyMessage(string propertyName, object value) : base(value, propertyName)
    {
    }
}