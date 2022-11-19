using System.Drawing;

namespace SketchOverlay.Native.Input.Mouse;

public class MouseButtonEventArgs : EventArgs
{
    internal MouseButtonEventArgs(Point pos, MouseButton button)
    {
        if (button == MouseButton.XButton)
            throw new ArgumentOutOfRangeException(nameof(button), 
                $"Primary constructor is not compatible with XButton");

        Position = pos;
        Button = button;
        XButtonIndex = null;
    }
    
    internal MouseButtonEventArgs(Point pos, int xButtonIndex)
    {
        Position = pos;
        Button = MouseButton.XButton;
        XButtonIndex = xButtonIndex;
    }
    
    public Point Position { get; }
    public MouseButton Button { get; }
    public int? XButtonIndex { get; } 
}