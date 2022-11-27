namespace SketchOverlay.Messages;

public class DrawingWindowSetPropertyMessage : SimpleSetPropertyMessage
{
    public DrawingWindowSetPropertyMessage(string propertyName, object value) : base(value, propertyName)
    {
    }
}