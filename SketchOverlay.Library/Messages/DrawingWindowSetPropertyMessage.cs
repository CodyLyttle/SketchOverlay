namespace SketchOverlay.Library.Messages;

public class DrawingWindowSetPropertyMessage : SimpleSetPropertyMessage
{
    public DrawingWindowSetPropertyMessage(string propertyName, object value) : base(value, propertyName)
    {
    }
}